import { useParams, useNavigate } from "react-router-dom";
import "../css/WriteReview.css";
import { useState, useEffect } from "react";
import axios from "axios";

const API_KEY = "51cd4d4953ec1657016f07763c6b902a";

function WriteReview() {
  const { id } = useParams();
  const navigate = useNavigate();

  // Track rating selection and hover state for interactive stars
  const [rating, setRating] = useState(0);
  const [hoverRating, setHoverRating] = useState(0);
  const [reviewText, setReviewText] = useState("");
  const [movie, setMovie] = useState(null);

  //
  useEffect(() => {
    async function loadMovie() {
        const res = await fetch(
            `https://api.themoviedb.org/3/movie/${id}?api_key=${API_KEY}`
        );
        const data = await res.json();
        setMovie(data);
    }

    loadMovie();
}, [id]);

  // Handles review form submission - validates rating and navigates back to movie detail
  const submitReview = (e) => {
    e.preventDefault();

    if (rating < 1 || rating > 5) {
      alert("Please select a star rating between 1 and 5!");
      return;
    }

    var writesData = {
      MovieId: id,
      MovieTitle: movie.title,
      Rating: rating,
      Comment: reviewText
  }

  axios.post("http://localhost:5210/api/writes/add", writesData, {withCredentials: true})
  .then(res => {console.log(res.data)})
  .catch(err => {console.log(err)})

    

    // TODO: Send reviewData to backend
    alert("Review submitted!");
    navigate(`/movie/${id}`);
  };

//   const addUserWrites = () => {

//     var writesData = {
//         MovieId: id,
//         Rating: rating,
//         Comment: reviewText
//     }

//     axios.post("http://localhost:5210/api/writes/add", writesData, {withCredentials: true})
//     .then(res => {console.log(res.data)})
//     .catch(err => {console.log(err)})

// }

  return (
    <div className="write-review-page">
      <div className="write-review-container">

        <h1 className="review-title">Write a Review</h1>

        {/* Interactive star rating system */}
        <div className="review-rating-section">
          <h3>Select Rating</h3>
          <div className="interactive-stars">
            {[1, 2, 3, 4, 5].map((num) => (
              <span
                key={num}
                className={`star ${(hoverRating || rating) >= num ? "full" : "empty"}`}
                onMouseEnter={() => setHoverRating(num)}
                onMouseLeave={() => setHoverRating(0)}
                onClick={() => setRating(num)}
              >
                ★
              </span>
            ))}
          </div>
        </div>

        <form onSubmit={submitReview} className="review-form">
          <textarea
            className="review-input"
            placeholder="Write your thoughts about the movie..."
            value={reviewText}
            onChange={(e) => setReviewText(e.target.value)}
            required
          />

          <div className="review-buttons">
            <button
              type="button"
              className="cancel-button"
              onClick={() => navigate(`/movie/${id}`)}
            >
              Cancel
            </button>

            <button onClick={submitReview} type="submit" className="submit-review-button">
              Submit Review
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default WriteReview;