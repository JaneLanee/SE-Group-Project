import { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import MovieCard from "../components/MovieCard";
import "../css/Home.css";

const API_KEY = "51cd4d4953ec1657016f07763c6b902a";

export default function Home() {
  const [movies, setMovies] = useState([]);
  const [recommendedMovies, setRecommendedMovies] = useState([]);
  const [query, setQuery] = useState("");
  const [isSearching, setIsSearching] = useState(false);
  const location = useLocation();

  useEffect(() => {
    setIsSearching(false);
    setQuery("");
    
    fetch(`https://api.themoviedb.org/3/movie/popular?api_key=${API_KEY}`)
      .then(res => res.json())
      .then(data => setMovies(data.results || []))
      .catch(err => console.error("Fetch error:", err));

  }, [location]);

  function handleSearch(e) {
    e.preventDefault();
    if (!query.trim()) {
      setIsSearching(false);
      fetch(`https://api.themoviedb.org/3/movie/popular?api_key=${API_KEY}`)
        .then(res => res.json())
        .then(data => setMovies(data.results || []))
        .catch(err => console.error("Fetch error:", err));
      return;
    }

    setIsSearching(true);
    fetch(
      `https://api.themoviedb.org/3/search/movie?api_key=${API_KEY}&query=${query}`
    )
      .then(res => res.json())
      .then(data => setMovies(data.results || []))
      .catch(err => console.error("Search error:", err));
  }

  return (
    <div className="home">
      <form className="search-form" onSubmit={handleSearch}>
        <input
          className="search-input"
          type="text"
          placeholder="Search movies..."
          value={query}
          onChange={e => setQuery(e.target.value)}
        />
        <button className="search-button">Search</button>
      </form>

      {!isSearching && (
        <div className="movie-row-container">
          <h2 className="row-title">Recommended For You</h2>
          <div className="movie-row">
            {recommendedMovies.length > 0 ? (
              recommendedMovies.map(movie => (
                <MovieCard
                  key={movie.id}
                  movie={{
                    id: movie.id,
                    title: movie.title,
                    release_date: movie.release_date,
                    url: `https://image.tmdb.org/t/p/w500${movie.poster_path}`,
                  }}
                />
              ))
            ) : (
              <p className="empty-message">No recommendations yet. Start reviewing movies to get personalized suggestions!</p>
            )}
          </div>
        </div>
      )}

      <div className="movie-row-container">
        <h2 className="row-title">{isSearching ? "Search Results" : "Popular Movies"}</h2>
        <div className="movie-row">
          {movies.length > 0 ? (
            movies.map(movie => (
              <MovieCard
                key={movie.id}
                movie={{
                  id: movie.id,
                  title: movie.title,
                  release_date: movie.release_date,
                  url: `https://image.tmdb.org/t/p/w500${movie.poster_path}`,
                }}
              />
            ))
          ) : (
            <p className="empty-message">No movies found.</p>
          )}
        </div>
      </div>
    </div>
  );
}