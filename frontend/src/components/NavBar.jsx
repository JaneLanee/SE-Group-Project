import {Link, useNavigate} from "react-router-dom";
import "../css/NavBar.css"

function NavBar() {
    const navigate = useNavigate();
    
    // TODO: Replace this with your actual authentication state
    // For now, this simulates checking if a user is logged in
    const isLoggedIn = true; // Change to true to test logged-in state
    
    const handleProfileClick = () => {
        if (isLoggedIn) {
            navigate("/profile");
        } else {
            navigate("/login");
        }
    };
    
    return <nav className="navbar">
        <div className="navbar-brand">
            <Link to="/">WatchList</Link>
        </div>
        <div className="navbar-links">
            <Link to ="/" className="nav-link">Home</Link>
            <Link to ="/favorites" className="nav-link">Favorites</Link>
            <button onClick={handleProfileClick} className="nav-link profile-button">
                Profile
            </button>
        </div>
    </nav>
}

export default NavBar