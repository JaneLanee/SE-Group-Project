import './css/App.css';
import Home from "./pages/Home"
import {Routes, Route, BrowserRouter} from "react-router-dom"
import Favorites from "./pages/Favorites";
import NavBar from './components/NavBar';
import MovieDetail from './pages/MovieDetail';

function App() {
  return (
      <BrowserRouter>
          <div>
              <NavBar />
                <main className="main-content">
                    <Routes>
                        <Route path="/" element={<Home />}/>
                        <Route path="/favorites" element={<Favorites />}/>
                        <Route path="/movie/:id" element={<MovieDetail />}/>
                    </Routes>
                </main>
          </div>
      </BrowserRouter>
  );
}

export default App;
