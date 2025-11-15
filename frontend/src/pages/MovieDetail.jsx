import { useParams, useNavigate } from "react-router-dom";
import "../css/MovieDetail.css";

function MovieDetail() {
    const { id } = useParams();
    const navigate = useNavigate();

    const renderStars = (rating) => {
        const stars = [];
        const wholeRating = Math.round(rating);

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

    return (
        <div className="movie-detail">
            <div className="movie-detail-container">

                <div className="movie-poster-section">
                    <div className="detail-poster">
                        <div className="poster-placeholder">
                            <span>Movie Poster</span>
                        </div>
                        <button className="favorite-btn-detail">
                            ♡ Favorite
                        </button>
                        <button className="watchlist-btn-detail">
                            📑 Add to Watchlist
                        </button>
                    </div>
                </div>

                <div className="movie-info-section">
                    <h1 className="movie-title">MOVIE TITLE</h1>

                    <div className="movie-description">
                        <h3>DESCRIPTION</h3>
                        <p>
                            MOVIE DESCRIPTION
                        </p>
                    </div>

                    <div className="recommended-section">
                        <h3>RECOMMENDED</h3>
                        <div className="recommended-movies">
                            <div className="recommended-card">Movie 1</div>
                            <div className="recommended-card">Movie 2</div>
                            <div className="recommended-card">Movie 3</div>
                        </div>
                    </div>
                </div>
                
                <div className="movie-review-section">

                    <div className="rating-section">
                        <h3>RATING</h3>
                        <div className="star-rating">{renderStars(3.0)}</div>
                        <div className="rating-display">
                            <span className="rating-value">3.0/5</span>
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
                            <p>No reviews yet</p>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    );
}

export default MovieDetail;
