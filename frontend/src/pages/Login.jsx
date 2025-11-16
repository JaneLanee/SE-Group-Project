import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../css/Login.css";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();


  //Handle login submission form
  const submitLogin = (e) => {
    e.preventDefault();
    
    if (!email || !password) {
      alert("Please fill in all fields!");
      return;
    }

    alert("Login feature coming soon!");
  };

  return (
    <div className="login-page">
      <div className="login-box">
        <h1 className="login-title">Welcome Back</h1>
        <p className="login-subtitle">Sign in to WatchList</p>

        <form onSubmit={submitLogin} className="login-form">
          <div className="input-group">
            <label>Email</label>
            <input
              type="email"
              placeholder="Enter your email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div className="input-group">
            <label>Password</label>
            <input
              type="password"
              placeholder="Enter your password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="login-button">
            Sign In
          </button>
        </form>

        <div className="login-footer">
          <p>
            Don't have an account?{" "}
            <span className="signup-link" onClick={() => navigate("/signup")}>
              Sign up
            </span>
          </p>
        </div>
      </div>
    </div>
  );
}

export default Login;
