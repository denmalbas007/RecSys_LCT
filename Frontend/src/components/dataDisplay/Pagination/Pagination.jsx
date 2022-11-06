import cl from "./Pagination.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAngleLeft, faAngleRight } from "@fortawesome/free-solid-svg-icons";

const Pagination = ({ page, setPage, totalPages }) => {
  const handlePageChange = (newPage) => {
    setPage(newPage);
  };
  const pages = Array.from({ length: totalPages }, (_, i) => i + 1);
  const pagesToShow = pages.length > 5 ? pages.slice(0, 5) : pages;

  return (
    <div className={cl.pagination}>
      <button
        className={cl.pagination_chevron}
        onClick={() => handlePageChange(page - 1)}
        disabled={page === 1}
      >
        <FontAwesomeIcon icon={faAngleLeft} />
      </button>
      {pagesToShow.map((p) => (
        <button
          key={p}
          className={[cl.pagination_button, p === page ? cl.active : ""].join(
            " "
          )}
          onClick={() => handlePageChange(p)}
          disabled={p === page}
        >
          {p}
        </button>
      ))}
      {pages.length > 5 && <span>...</span>}
      {/* last page */}
      {pages.length > 5 && (
        <button
          className={[
            cl.pagination_button,
            pages.length === page ? cl.active : "",
          ].join(" ")}
          onClick={() => handlePageChange(pages.length)}
          disabled={pages.length === page}
        >
          {pages.length}
        </button>
      )}
      <button
        className={cl.pagination_chevron}
        onClick={() => handlePageChange(page + 1)}
        disabled={page === totalPages}
      >
        <FontAwesomeIcon icon={faAngleRight} />
      </button>
    </div>
  );
};

export default Pagination;
