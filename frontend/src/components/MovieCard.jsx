import "../css/MovieCard.css";
import { useNavigate } from "react-router-dom";

function MovieCard({ movie }) {
    const navigate = useNavigate();

    //Handles favorite button click
    //TODO: Connect the backend and send the favorites down when User click
    function onClickFavorite(e) {
        e.stopPropagation();
        alert("clicked favorite");
    }
    
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

