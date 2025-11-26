import { useNavigate } from "react-router-dom";
import "../css/UserPage.css";
import { useEffect, useState } from "react";
import axios from "axios";

// User profile page - displays user info, watchlist, and activity options
function UserPage() {
  const navigate = useNavigate();

  const [movies, setMovies] = useState([]);


  useEffect(() => {
    axios.get("http://localhost:5210/api/watchlist/userwatchlist", {withCredentials: true})
    .then(res => {setMovies(res.data)})
    .catch(err => {console.log(err)})
  }, [])

  console.log(movies);

  return (
    <div className="user-page">
      <div className="user-page-container">
        
        {/* Left sidebar - profile picture and bio */}
        <div className="user-info-section">
          <div className="profile-picture-placeholder">
            <span>Profile Picture</span>
          </div>
          <div className="profile-description">
            <h2> BIO </h2>
            <p> USER BIO </p>
          </div>
        </div>

        {/* Main content - user info, activity buttons, and watchlist */}
        <div className="profile-info-section">
          <div className="user-heading">
            <h1> USER NAME </h1>
            <h2> 20 FOLLOWERS </h2>
            <h2> 20 FOLLOWING </h2>
            <button 
              className="settings-button"
              onClick={() => navigate("/settings")}
            >
              Profile Settings
            </button>
          </div>
          
          {/* Activity section with navigation to favorites and reviews */}
          <div className="user-activity">
            <div className="your-watchlist-title">
              <h3> YOUR WATCHLIST </h3>
            </div>
            <button className="view-favorites-button">
              View Your Favorites
            </button>
            <button className="view-reviews-button">View Your Reviews</button>
          </div>
          
          {/* User's watchlist display */}
          <div className="user-watchlist">
            <div className="watchlist-card">Movie 1</div>
            <div className="watchlist-card">Movie 2</div>
            <div className="watchlist-card">Movie 3</div>
            <div className="watchlist-card">Movie 4</div>
            <div className="watchlist-card">Movie 5</div>
            {}
          </div>
        </div>
      </div>
    </div>
  );
}

export default UserPage;