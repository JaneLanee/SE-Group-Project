import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../css/Signup.css";

import axios from "axios";

// Signup page - handles new user registration
function Signup() {

  //axios.defaults.withCredentials = true;
  // Form input states
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [dob, setDob] = useState("");
  const navigate = useNavigate();

  // Handles signup form submission - validates passwords and creates account
  const submitSignup = (e) => {
    e.preventDefault();

    if (password !== confirmPassword) {
      alert("Passwords don't match!");
      return;
    }

    if (password.length < 6) {
      alert("Password must be at least 6 characters!");
      return;
    }

    var userData = {
      username: username,
      email: email,
      password: password,
      dateOfBirth: dob
    }

    // TODO: Send signup data to backend
    axios.post("http://localhost:5210/api/user/signup", userData, {
      withCredentials: true,
      headers: {'Content-Type': 'application/json'}
    }).then(res => {
      console.log(res);
      navigate('/');
    }).catch( error => {
      console.log(error);
      navigate('/signup');
    })
  };

  return (
    <div className="signup-page">
      <div className="signup-box">
        <h1 className="signup-title">Create Account</h1>
        <p className="signup-subtitle">Join WatchList today</p>

        <form onSubmit={submitSignup} className="signup-form">
          <div className="input-group">
            <label>Username</label>
            <input
              type="text"
              placeholder="Choose a username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>

          <div className="input-group">
            <label>Email</label>
            <input
              type="email"
              name="Email"
              placeholder="Enter your email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div className="input-group">
            <label>Date of Birth</label>
            <input
              type="date"
              name="DateOfBirth"
              value={dob}
              onChange={(e) => setDob(e.target.value)}
              required
            />
          </div>

          <div className="input-group">
            <label>Password</label>
            <input
              type="password"
              name="Password"
              placeholder="Create a password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <div className="input-group">
            <label>Confirm Password</label>
            <input
              type="password"
              placeholder="Confirm your password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="signup-button">
            Create Account
          </button>
        </form>

        <div className="signup-footer">
          <p>
            Already have an account?{" "}
            <span className="login-link" onClick={() => navigate("/login")}>
              Sign in
            </span>
          </p>
        </div>
      </div>
    </div>
  );
}

export default Signup;
