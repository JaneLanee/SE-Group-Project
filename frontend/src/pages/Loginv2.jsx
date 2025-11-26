import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../css/Signup.css";

import axios from "axios";

// Signup page - handles new user registration
function Loginv2() {

  //axios.defaults.withCredentials = true;
  // Form input states
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  // Handles signup form submission - validates passwords and creates account
  const submitLogin = (e) => {
    e.preventDefault();


    if (password.length < 6) {
      alert("Password must be at least 6 characters!");
      return;
    }

    var userData = {
      username: username,
      password: password,
    }

    // TODO: Send signup data to backend
    axios.post("http://localhost:5210/api/user/login", userData, {
      withCredentials: true,
      headers: {'Content-Type': 'application/json'}
    }).then(res => {
      console.log(res);
      navigate('/');
    }).catch( error => {
      console.log(error);
      navigate('/login');
    })

  };

  return (
    <div className="signup-page">
      <div className="signup-box">
        <h1 className="signup-title">Login</h1>
        <p className="signup-subtitle">Log into WatchList</p>

        <form onSubmit={submitLogin} className="signup-form">
          <div className="input-group">
            <label>Username</label>
            <input
              type="text"
              placeholder="Enter username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>

          <div className="input-group">
            <label>Password</label>
            <input
              type="password"
              name="Password"
              placeholder="Enter password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>


          <button type="submit" className="signup-button">
            Login
          </button>
        </form>

        <div className="signup-footer">
          <p>
            Don't have an account?{" "}
            <span className="login-link" onClick={() => navigate("/signup")}>
              Signup
            </span>
          </p>
        </div>
      </div>
    </div>
  );
}

export default Loginv2;
