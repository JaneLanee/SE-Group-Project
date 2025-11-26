import { useParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import "../css/MovieDetail.css";

import axios from "axios";

const API_KEY = "51cd4d4953ec1657016f07763c6b902a";

function MovieDetail() {
    const { id } = useParams();
    const navigate = useNavigate();

    const [movie, setMovie] = useState(null);
    const [recommendations, setRecommendations] = useState([]);
    const [user, setUser] = useState({});
    const [writes, setWrites] = useState([]);

    // fetch movie details from API
    // TODO: fetch recommended movies from backend, right now just blank recommendation fetch for API
    useEffect(() => {
        async function loadMovie() {
            const res = await fetch(
                `https://api.themoviedb.org/3/movie/${id}?api_key=${API_KEY}`
            );
            const data = await res.json();
            setMovie(data);
        }

        async function loadRecommendations() {
            const res = await fetch(
                `https://api.themoviedb.org/3/movie/${id}/recommendations?api_key=${API_KEY}`
            );
            const data = await res.json();
            setRecommendations(data.results || []);
        }

        loadMovie();
        loadRecommendations();
    }, [id]);

    useEffect(() => {
        //FETCH LOGGED IN USER INFO
        axios.get("http://localhost:5210/api/user/loggedUser", {withCredentials: true, headers: {'Content-Type': 'application/json'}})
        .then(res => setUser(res.data))
        .catch(err => console.log(err))
    },[])

    console.log(parseInt(user.userId));
    console.log(movie)

    const addMovieToWatchlist = () => {

        var userInt = parseInt(user.UserId)

        var watchlistData = {
            UserId: userInt,
            MovieId: movie.id,
            MovieTitle: movie.title
        }

        axios.post("http://localhost:5210/api/watchlist/add", watchlistData, {withCredentials: true})
        .then(res => console.log(res))
        .catch(err => console.log(err)
        )
    }

    //
    useEffect(() => {

        const movieWritesData = {
            movieId: id
        }

        axios.get(`http://localhost:5210/api/writes/${id}`, {withCredentials: true})
        .then(res => {setWrites(res.data)})
        .catch(err => {console.log(err)})

    }, [id])

    console.log(writes);

    //Converts 0-10 rating to 5-star scale and display
    const renderStars = (rating) => {
        const stars = [];
        const wholeRating = Math.round(rating / 2);

        for (let i = 1; i <= 5; i++) {
            stars.push(
                <span
                    key={i}
                    className={`star ${i <= wholeRating ? "full" : "empty"}`}
                >
                    ★
                </span>
            );
        }
        return stars;
    };

    if (!movie) {
        return <p style={{ color: "white", textAlign: "center" }}>Loading...</p>;
    }


    return (
        <div className="movie-detail">
            <div className="movie-detail-container">
                {/* Left column: Poster and action buttons */}
                <div className="movie-poster-section">
                    <div className="detail-poster">
                        <img
                            src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                            alt={movie.title}
                            className="poster-img"
                        />
                    </div>
                    <button className="favorite-btn-detail">
                        ♡ Favorite
                        </button>
                    <button onClick={addMovieToWatchlist} className="watchlist-btn-detail">
                        📑 Add to Watchlist
                    </button>
                </div>
                {/* Middle column: Title, description, and recommendations */}
                <div className="movie-info-section">
                    <h1 className="movie-title">{movie.title}</h1>

                    <div className="movie-description">
                        <h3>DESCRIPTION</h3>
                        <p>
                            {movie.overview}
                        </p>
                    </div>

                    <div className="recommended-section">
                        <h3>RECOMMENDED</h3>
                        <div className="recommended-movies">
                            {recommendations.slice(0, 6).map((r) => (
                                <div
                                    key={r.id}
                                    className="recommended-card"
                                    onClick={() => navigate(`/movie/${r.id}`)}
                                >
                                    {r.poster_path ? (
                                        <img
                                            src={`https://image.tmdb.org/t/p/w200${r.poster_path}`}
                                            alt={r.title}
                                            className="recommended-poster"
                                        />
                                    ) : (
                                        <div className="recommended-placeholder">
                                            No Image
                                        </div>
                                    )}
                                    <div className="recommended-title">{r.title}</div>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
                {/* Right column: Rating, review button, and reviews list */}
                <div className="movie-review-section">

                    <div className="rating-section">
                        <h3>RATING</h3>
                        <div className="star-rating">
                            {renderStars(movie.vote_average)}
                        </div>
                        <div className="rating-display">
                            <span className="rating-value">
                                {(movie.vote_average / 2).toFixed(1)}/5
                            </span>
                        </div>
                    </div>

                    <div className="review-actions">
                        <button
                            className="write-review-btn"
                            onClick={() => navigate(`/movie/${id}/review`)}
                        >
                            WRITE REVIEW
                        </button>
                    </div>

                    <div className="reviews-section">
                        <h3>REVIEWS</h3>
                        <div className="reviews-list">
                            {writes.length != 0 ? writes.map(write => (
                                <div>
                                    <p>{write.comment}</p>
                                </div>
                            )) : <p>No reviews yet</p>}
                        </div>
                    </div>

                </div>
            </div>
        </div>
    );
}

export default MovieDetail;