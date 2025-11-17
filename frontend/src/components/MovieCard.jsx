import "../css/MovieCard.css";
import { useNavigate } from "react-router-dom";

// Reusable movie card component - displays movie poster, title, and favorite button
function MovieCard({ movie }) {
    const navigate = useNavigate();

    // Handles favorite button click - stops event from triggering card navigation
    // TODO: Connect to backend to save/remove favorite
    function onClickFavorite(e) {
        e.stopPropagation();
        alert("clicked favorite");
    }
    
    // Navigates to the movie detail page
    function onClickMovie() {
        navigate(`/movie/${movie.id}`);
    }

    return (
        <div className="movie-card" onClick={onClickMovie}>
            <div className="movie-poster">
                <img src={movie.url} alt={movie.title} />
                <div className="movie-overlay">
                    <button className="favorite-btn" onClick={onClickFavorite}>
                        ♡
                    </button>
                </div>
            </div>
            <div className="movie-info">
                <h3>{movie.title}</h3>
                <p>{movie.release_date}</p>
            </div>
        </div>
    );
}

export default MovieCard;