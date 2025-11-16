import { useParams, useNavigate } from "react-router-dom";
import "../css/WriteReview.css";
import { useState } from "react";

function WriteReview() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [rating, setRating] = useState(0);
  const [hoverRating, setHoverRating] = useState(0);
  const [reviewText, setReviewText] = useState("");

  //Handles review form submission
  //Validates rating, preps review data, and goes back to movie detail
  const submitReview = (e) => {
    e.preventDefault();

    if (rating < 1 || rating > 5) {
      alert("Please select a star rating between 1 and 5!");
      return;
    }

    const reviewData = {
      movieId: id,
      rating,
      text: reviewText,
    };

    alert("Review submitted!");
    navigate(`/movie/${id}`);
  };

  return (
    <div className="write-review-page">
      <div className="write-review-container">

        <h1 className="review-title">Write a Review</h1>

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

            <button type="submit" className="submit-review-button">
              Submit Review
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default WriteReview;
