use watchlist; 

-- Users table 
CREATE TABLE Users (
 User_Id INT AUTO_INCREMENT PRIMARY KEY,
 Username VARCHAR(50) UNIQUE NOT NULL,
 Email VARCHAR(255) UNIQUE NOT NULL,
 Password_hash VARCHAR(255) NOT NULL,
 DoB DATE, 
 created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
 mfa_enabled BOOLEAN DEFAULT FALSE,
 failed_attempts INT DEFAULT 0,
 last_attempt_time TIMESTAMP NULL,
 account_locked BOOLEAN DEFAULT FALSE,
 INDEX idx_username (Username), -- faster loookup 
 INDEX idx_email (Email)
) ENGINE=InnoDB;
 
 -- Sessions table 
CREATE TABLE Sessions (
 Session_Id INT AUTO_INCREMENT PRIMARY KEY,
 User_Id INT NOT NULL,
 token_hash VARCHAR(255) UNIQUE NOT NULL,
 ip_address VARCHAR(45),
 is_active BOOLEAN DEFAULT TRUE,
 created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
 expires_at TIMESTAMP,
 FOREIGN KEY (User_Id) REFERENCES Users(User_Id) ON DELETE CASCADE,
 INDEX idx_token (token_hash),
 INDEX idx_user (User_Id)
) ENGINE=InnoDB;

-- Genres table 
CREATE TABLE Genres (
 Genre_Id INT AUTO_INCREMENT PRIMARY KEY,
 genre_name VARCHAR(100) UNIQUE NOT NULL,
 description TEXT,
 INDEX idx_genre_name (genre_name)
) ENGINE=InnoDB;

-- Movies table 
CREATE TABLE Movies (
 Movie_Id INT PRIMARY KEY,  -- TMDB API id
 title VARCHAR(255) NOT NULL,
 poster_url VARCHAR(500), 
 release_year YEAR,
 last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
 INDEX idx_title (title),
 INDEX idx_release_year (release_year) 
) ENGINE=InnoDB;

-- Belongs to table between movies and genre (M to M)
CREATE TABLE BelongsTo (
 Movie_Id INT NOT NULL,
 Genre_Id INT NOT NULL,
 PRIMARY KEY (Movie_Id, Genre_Id),
 FOREIGN KEY (Movie_Id) REFERENCES Movies(Movie_Id) ON DELETE CASCADE,
 FOREIGN KEY (Genre_Id) REFERENCES Genres(Genre_Id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- Friends table 
CREATE TABLE Friends (
 User_Id INT NOT NULL,
 Friend_User_Id INT NOT NULL,
 Username VARCHAR(50) NOT NULL,
 created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
 PRIMARY KEY (User_Id, Friend_User_Id),
 FOREIGN KEY (User_Id) REFERENCES Users(User_Id) ON DELETE CASCADE,
 FOREIGN KEY (Friend_User_Id) REFERENCES Users(User_Id) ON DELETE CASCADE,
 INDEX idx_user (User_Id),
 INDEX idx_friend (Friend_User_Id)
) ENGINE=InnoDB;

-- WatchList table between users ande movies (M to M)
CREATE TABLE Watchlist (
 User_Id INT NOT NULL,
 Movie_Id INT NOT NULL,
 status ENUM('want_to_watch', 'watching', 'completed') DEFAULT 'want_to_watch',
 personal_rating DECIMAL(3,1) CHECK (personal_rating >= 0 AND personal_rating <= 10),
 date_added TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
 PRIMARY KEY (User_Id, Movie_Id),
 FOREIGN KEY (User_Id) REFERENCES Users(User_Id) ON DELETE CASCADE,
 FOREIGN KEY (Movie_Id) REFERENCES Movies(Movie_Id) ON DELETE CASCADE,
 INDEX idx_status (status),
 INDEX idx_date_added (date_added)
) ENGINE=InnoDB;

-- Writes table(reviews) between users and movies (M to M) 
CREATE TABLE Writes (
 User_Id INT NOT NULL,
 Movie_Id INT NOT NULL,
 rating DECIMAL(3,1) NOT NULL CHECK (rating >= 0 AND rating <= 10),
 comment TEXT,
 date_posted TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
 upvote_count INT DEFAULT 0,
 PRIMARY KEY (User_Id, Movie_Id),
 FOREIGN KEY (User_Id) REFERENCES Users(User_Id) ON DELETE CASCADE,
 FOREIGN KEY (Movie_Id) REFERENCES Movies(Movie_Id) ON DELETE CASCADE,
 INDEX idx_rating (rating),
 INDEX idx_date_posted (date_posted),
 INDEX idx_upvotes (upvote_count)
) ENGINE=InnoDB;

-- create a new user
DELIMITER //
CREATE PROCEDURE createUser(IN m_username VARCHAR(50),IN m_email VARCHAR(255),IN m_password_hash VARCHAR(255),IN m_dob DATE)
BEGIN
    INSERT INTO Users (Username, Email, Password_hash, DoB)
    VALUES (m_username, m_email, m_password_hash, m_dob);
END;
// DELIMITER ;

-- delete a user
DELIMITER //
CREATE PROCEDURE deleteUser(IN m_user_id INT)
BEGIN
    DELETE FROM Users
    WHERE User_Id = m_user_id;
END;
// DELIMITER ;

-- get full user profile
DELIMITER //
CREATE PROCEDURE getFullUser(IN m_user_id INT)
BEGIN
	SELECT User_Id, Username, Email, Password_hash, DoB, created_at, mfa_enabled, failed_attempts, last_attempt_time, account_locked
    FROM Users
    WHERE User_Id = m_user_id;
END;
// DELIMITER ;


-- get user information
DELIMITER //
CREATE PROCEDURE getUser(IN m_user_id INT)
BEGIN
	SELECT User_Id, Username, Email, Password_hash, DoB
    FROM Users
    WHERE User_Id = m_user_id;
END;
// DELIMITER ;


-- update username
DELIMITER //
CREATE PROCEDURE updateUsername(IN m_user_id INT, IN m_new_username VARCHAR(50))
BEGIN
	UPDATE Users
    SET Username = m_new_username
    WHERE User_Id = m_user_id;
END;
// DELIMITER ;

-- update email
DELIMITER //
CREATE PROCEDURE updateEmail(IN m_user_id INT, IN m_new_email VARCHAR(255))
BEGIN
	UPDATE Users
    SET Email = m_new_email
    WHERE User_Id = m_user_id;
END;
// DELIMITER ;

-- update password where user has not been locked out of their account (CS can update as needed)
DELIMITER //
CREATE PROCEDURE updatePassword(IN m_user_id INT, IN m_new_password_hash VARCHAR(255))
BEGIN
	UPDATE Users
    SET password_hash = m_new_password_hash,
		failed_attempts = 0,
        account_locked = FALSE
    WHERE User_Id = m_user_id;
END;
// DELIMITER ;

-- update dob
DELIMITER //
CREATE PROCEDURE updateDoB(IN m_user_id INT, IN m_new_dob DATE)
BEGIN
	UPDATE Users
    SET DoB = m_new_dob
    WHERE User_Id = m_user_id;
END;
// DELIMITER ;

-- get user's friends list
DELIMITER //
CREATE PROCEDURE getUserFriends(IN m_user_id INT)
BEGIN
    SELECT f.Friend_User_Id, u.Username, u.Email
    FROM Friends f
    JOIN Users u ON f.Friend_User_Id = u.User_Id
    WHERE f.User_Id = m_user_id;
END;
// DELIMITER ;

-- get user's activity (watchlist and reviews)
DELIMITER //
CREATE PROCEDURE getUserActivitySummary(IN m_user_id INT)
BEGIN
    SELECT 
        (SELECT COUNT(*) 
        FROM Watchlist 
        WHERE User_Id = m_user_id) 
        AS total_watchlist,
        
        (SELECT COUNT(*) 
        FROM Writes WHERE 
        User_Id = m_user_id) 
        AS total_reviews;
END;
// DELIMITER ;

-- add a new movie
DELIMITER //
CREATE PROCEDURE addMovie(IN m_movie_id INT, IN m_title VARCHAR(255), IN m_poster_url VARCHAR(500), IN m_release_year YEAR)
BEGIN
    INSERT INTO Movies (Movie_Id, title, poster_url, release_year)
    VALUES (m_movie_id, m_title, m_poster_url, m_release_year);
END;
// DELIMITER ;

-- delete a movie
DELIMITER //
CREATE PROCEDURE deleteMovie(IN m_movie_id INT)
BEGIN
    DELETE FROM Movies WHERE Movie_Id = m_movie_id;
END;
// DELIMITER ;

-- update a movie
DELIMITER //
CREATE PROCEDURE updateMovieInfo(IN m_movie_id INT, IN m_title VARCHAR(255), IN m_poster_url VARCHAR(500), IN m_release_year YEAR)
BEGIN
    UPDATE Movies
    SET title = m_title,
        poster_url = m_poster_url,
        release_year = m_release_year
    WHERE Movie_Id = m_movie_id;
END;
// DELIMITER ;

-- get movie details
DELIMITER //
CREATE PROCEDURE getMovieDetails(IN m_movie_id INT)
BEGIN
    SELECT * FROM Movies 
    WHERE Movie_Id = m_movie_id;
END;
// DELIMITER ;

-- search movies by title
DELIMITER //
CREATE PROCEDURE searchMovieTitle(IN m_title VARCHAR(255))
BEGIN
    SELECT * FROM Movies 
    WHERE title LIKE CONCAT('%', m_title, '%');
END;
// DELIMITER ;

-- search movies by genre
DELIMITER //
CREATE PROCEDURE searchMovieGenre(IN m_genre_id INT)
BEGIN
    SELECT m.Movie_Id, m.title, m.release_year
    FROM Movies m
    JOIN BelongsTo b ON m.Movie_Id = b.Movie_Id
    WHERE b.Genre_Id = m_genre_id;
END;
// DELIMITER ;

-- get top rated movies
DELIMITER //
CREATE PROCEDURE getTopRatedMovies(IN m_limit INT)
BEGIN
    SELECT m.title, ROUND(AVG(w.rating),2) AS avg_rating, COUNT(w.User_Id) AS review_count
    FROM Movies m
    JOIN Writes w ON m.Movie_Id = w.Movie_Id
    GROUP BY m.Movie_Id
    ORDER BY avg_rating DESC
    LIMIT m_limit;
END;
// DELIMITER ;

-- add friend
DELIMITER //
CREATE PROCEDURE addFriend(IN m_user_id INT, IN m_friend_id INT)
BEGIN
    INSERT INTO Friends (User_Id, Friend_User_Id, Username)
    SELECT m_user_id, m_friend_id, Username 
		FROM Users 
        WHERE User_Id = m_friend_id;
    INSERT INTO Friends (User_Id, Friend_User_Id, Username)
    SELECT m_friend_id, m_user_id, Username 
		FROM Users 
		WHERE User_Id = m_user_id;
END;
// DELIMITER ;

-- remove friend
DELIMITER //
CREATE PROCEDURE removeFriend(IN m_user_id INT, IN m_friend_id INT)
BEGIN
    DELETE FROM Friends
    WHERE (User_Id = m_user_id AND Friend_User_Id = m_friend_id)
       OR (User_Id = m_friend_id AND Friend_User_Id = m_user_id);
END;
// DELIMITER ;

-- get user's friends list
DELIMITER //
CREATE PROCEDURE getFriendsList(IN m_user_id INT)
BEGIN
    SELECT f.Friend_User_Id, u.Username, u.Email
    FROM Friends f
    JOIN Users u ON f.Friend_User_Id = u.User_Id
    WHERE f.User_Id = m_user_id;
END;
// DELIMITER ;

-- get user's mutual friends
DELIMITER //
CREATE PROCEDURE getMutualFriends(IN m_user1 INT, IN m_user2 INT)
BEGIN
    SELECT u.Username, u.User_Id
    FROM Friends f1
    JOIN Friends f2 ON f1.Friend_User_Id = f2.Friend_User_Id
    JOIN Users u ON u.User_Id = f1.Friend_User_Id
    WHERE f1.User_Id = m_user1 AND f2.User_Id = m_user2;
END;
// DELIMITER ;

-- recommend friends' high ranked movies
DELIMITER //
CREATE PROCEDURE recommendMoviesFromFriends(IN m_user_id INT)
BEGIN
    SELECT DISTINCT m.title, m.release_year, w.rating
    FROM Writes w
    JOIN Movies m ON w.Movie_Id = m.Movie_Id
    WHERE w.User_Id IN (SELECT Friend_User_Id FROM Friends WHERE User_Id = m_user_id)
    AND w.rating >= 8.0
    ORDER BY w.rating DESC
    LIMIT 10;
END; // DELIMITER ;

-- add movie to watchlist
DELIMITER //
CREATE PROCEDURE addToWatchlist(IN m_user_id INT, IN m_movie_id INT, IN m_status ENUM('want_to_watch','watching','completed'))
BEGIN
    INSERT INTO Watchlist (User_Id, Movie_Id, status)
    VALUES (m_user_id, m_movie_id, m_status)
    ON DUPLICATE KEY UPDATE status = m_status;
END; // DELIMITER ;

-- remove movie from watchlist
DELIMITER //
CREATE PROCEDURE removeFromWatchlist(IN m_user_id INT, IN m_movie_id INT)
BEGIN
    DELETE FROM Watchlist 
    WHERE User_Id = m_user_id AND Movie_Id = m_movie_id;
END; // DELIMITER ;

-- update movie watchlist status
DELIMITER //
CREATE PROCEDURE updateWatchlistStatus(IN m_user_id INT, IN m_movie_id INT, IN m_status ENUM('want_to_watch','watching','completed'))
BEGIN
    UPDATE Watchlist
    SET status = m_status
    WHERE User_Id = m_user_id AND Movie_Id = m_movie_id;
END; // DELIMITER ;

-- add or edit rating
DELIMITER //
CREATE PROCEDURE updatePersonalRating(IN m_user_id INT, IN m_movie_id INT, IN m_rating DECIMAL(3,1))
BEGIN
    UPDATE Watchlist
    SET personal_rating = m_rating
    WHERE User_Id = m_user_id AND Movie_Id = m_movie_id;
END; // DELIMITER ;

-- get user watchlist
DELIMITER //
CREATE PROCEDURE getUserWatchlist(IN m_user_id INT)
BEGIN
    SELECT m.title, w.status, w.personal_rating, w.date_added
    FROM Watchlist w
    JOIN Movies m ON w.Movie_Id = m.Movie_Id
    WHERE w.User_Id = m_user_id
    ORDER BY w.date_added DESC;
END; // DELIMITER ;

-- get top rated movies in watchlist
DELIMITER //
CREATE PROCEDURE getPopularMoviesInWatchlists()
BEGIN
    SELECT m.title, COUNT(w.Movie_Id) AS added_count
    FROM Watchlist w
    JOIN Movies m ON w.Movie_Id = m.Movie_Id
    GROUP BY m.Movie_Id
    ORDER BY added_count DESC
    LIMIT 10;
END; // DELIMITER ;

-- get movies recently added to watchlist
DELIMITER //
CREATE PROCEDURE getRecentlyAddedMovies(IN m_user_id INT)
BEGIN
    SELECT m.title, w.date_added
    FROM Watchlist w
    JOIN Movies m ON w.Movie_Id = m.Movie_Id
    WHERE w.User_Id = m_user_id
    ORDER BY w.date_added DESC
    LIMIT 5;
END; // DELIMITER ;

-- add or update review
DELIMITER //
CREATE PROCEDURE writeReview(IN m_user_id INT, IN m_movie_id INT, IN m_rating DECIMAL(3,1), IN m_comment TEXT)
BEGIN
    INSERT INTO Writes (User_Id, Movie_Id, rating, comment)
    VALUES (m_user_id, m_movie_id, m_rating, m_comment)
    ON DUPLICATE KEY UPDATE rating = m_rating, comment = m_comment, date_posted = CURRENT_TIMESTAMP;
END;
// DELIMITER ;

-- delete review
DELIMITER //
CREATE PROCEDURE deleteReview(IN m_user_id INT, IN m_movie_id INT)
BEGIN
    DELETE FROM Writes
    WHERE User_Id = m_user_id AND Movie_Id = m_movie_id;
END;
// DELIMITER ;

-- list movie reviews
DELIMITER //
CREATE PROCEDURE getReviewsForMovie(IN m_movie_id INT)
BEGIN
    SELECT u.Username, w.rating, w.comment, w.date_posted, w.upvote_count
    FROM Writes w
    JOIN Users u ON w.User_Id = u.User_Id
    WHERE w.Movie_Id = m_movie_id
    ORDER BY w.upvote_count DESC, w.date_posted DESC;
END;
// DELIMITER ;

-- list user's reviews
DELIMITER //
CREATE PROCEDURE getUserReviews(IN m_user_id INT)
BEGIN
    SELECT m.title, w.rating, w.comment, w.date_posted
    FROM Writes w
    JOIN Movies m ON w.Movie_Id = m.Movie_Id
    WHERE w.User_Id = m_user_id
    ORDER BY w.date_posted DESC;
END;
// DELIMITER ;

-- increment upvotes
DELIMITER //
CREATE PROCEDURE upvoteReview(IN m_user_id INT, IN m_movie_id INT)
BEGIN
    UPDATE Writes
    SET upvote_count = upvote_count + 1
    WHERE User_Id = m_user_id AND Movie_Id = m_movie_id;
END;
// DELIMITER ;

-- compute movie's average rating
DELIMITER //
CREATE PROCEDURE getAverageRating(IN m_movie_id INT)
BEGIN
    SELECT m.title, ROUND(AVG(w.rating), 2) AS avg_rating, COUNT(w.User_Id) AS total_reviews
    FROM Movies m
    LEFT JOIN Writes w ON m.Movie_Id = w.Movie_Id
    WHERE m.Movie_Id = m_movie_id
    GROUP BY m.Movie_Id, m.title;
END;
// DELIMITER ;

-- get movies with most reviews along with their average ratings
DELIMITER //
CREATE PROCEDURE getTopReviewedMovies(IN m_limit INT)
BEGIN
    SELECT m.Movie_Id,m.title, ROUND(AVG(w.rating), 2) AS avg_rating, COUNT(w.User_Id) AS review_count
    FROM Movies m
    JOIN Writes w ON m.Movie_Id = w.Movie_Id
    GROUP BY m.Movie_Id, m.title
    ORDER BY review_count DESC, avg_rating DESC
    LIMIT m_limit;
END;
// DELIMITER ;

-- get friend's ratings
DELIMITER //
CREATE PROCEDURE getFriendsReviews(IN m_user_id INT)
BEGIN
    SELECT u.Username AS friend_name, m.title AS movie_title, w.rating, w.comment, w.date_posted
    FROM Writes w
    JOIN Users u ON w.User_Id = u.User_Id
    JOIN Movies m ON w.Movie_Id = m.Movie_Id
    WHERE w.User_Id IN (
		SELECT Friend_User_Id 
		FROM Friends 
		WHERE User_Id = m_user_id
    )
    ORDER BY w.date_posted DESC, w.rating DESC;
END;
// DELIMITER ;
