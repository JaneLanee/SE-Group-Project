import "./UserPage.css";

function UserPage() {
  return (
    <div className="user-page">
      <div className="user-page-container">
        <div className="user-info-section">
          <div className="profile-picture-placeholder">
            <span>Profile Picture</span>
          </div>
          <div className="profile-description">
            <h2> BIO </h2>
            <p> USER BIO </p>
          </div>
        </div>

        <div className="profile-info-section">
          <div className="user-heading">
            <h1> USER NAME </h1>
            <h2> 20 FOLLOWERS </h2>
            <h2> 20 FOLLOWING </h2>
            <button className="settings-button">Profile Settings</button>
          </div>
          <div className="user-activity">
            <div className="your-watchlist-title">
              <h3> YOUR WATCHLIST </h3>
            </div>
            <button className="view-favorites-button">
              View Your Favorites
            </button>
            <button className="view-reviews-button">View Your Reviews</button>
          </div>
          <div className="user-watchlist">
            <div className="watchlist-card">Movie 1</div>
            <div className="watchlist-card">Movie 2</div>
            <div className="watchlist-card">Movie 3</div>
            <div className="watchlist-card">Movie 4</div>
            <div className="watchlist-card">Movie 5</div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default UserPage;
