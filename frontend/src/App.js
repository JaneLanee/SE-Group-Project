//import logo from './logo.svg';
import './css/App.css';
import Home from "./pages/Home"
import {Routes, Route, BrowserRouter} from "react-router-dom"
import Favorites from "./pages/Favorites";
import NavBar from './components/NavBar';

function App() {
  return (
      <BrowserRouter>
          <div>
              <NavBar />
                <main className="main-content">
                    <Routes>
                        <Route path="/" element={<Home />}/>
                        <Route path="/favorites" element={<Favorites />}/>
                    </Routes>
                </main>
          </div>
      </BrowserRouter>
  );
}

export default App;
