import './css/App.css';
import Home from "./pages/Home";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Favorites from "./pages/Favorites";
import MovieDetail from "./pages/MovieDetail";
import WriteReview from "./pages/WriteReview";
import NavBar from "./components/NavBar";

function App() {
  return (
    <BrowserRouter>
      <div>
        <NavBar />
        <main className="main-content">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/favorites" element={<Favorites />} />
            <Route path="/movie/:id" element={<MovieDetail />} />
            <Route path="/movie/:id/review" element={<WriteReview />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

export default App;

