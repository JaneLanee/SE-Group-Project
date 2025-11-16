import "./UserSettings.css";

function UserSettings() {
  return (
    <div className="user-page">
      <div className="user-page-container">
        <div className="user-info-section">
          <div className="profile-picture-placeholder">
            <span>Profile Picture</span>
          </div>
          <button className="edit-profile-picture">
            {" "}
            Edit Profile Picture{" "}
          </button>
          <div className="profile-description">
            <h2> BIO </h2>
            <p> USER BIO </p>
            <button className="edit-biography">Edit Bio</button>
          </div>
        </div>

        <div className="profile-info-section">
          <div className="user-heading">
            <h1> USER NAME </h1>
            <h2> 20 FOLLOWERS </h2>
            <h2> 20 FOLLOWING </h2>
          </div>
          <div className="editing-options">
            <h3> Profile Settings </h3>
            <button className="edit-user-button">Edit Profile</button>
          </div>
          <div className="user-watchlist">
            <div className="watchlist-card">Movie 1</div>
            <div className="watchlist-card">Movie 2</div>
            <div className="watchlist-card">Movie 3</div>
            <div className="watchlist-card">Movie 4</div>
            <div className="watchlist-card">Movie 5</div>
          </div>
          <button className="edit-watchlist-button">Edit Watchlist</button>
        </div>
      </div>
    </div>
  );
}

export default UserSettings;
