import cl from "./SimplePagination.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAngleLeft, faAngleRight } from "@fortawesome/free-solid-svg-icons";

const SimplePagination = ({ page, nextPage, previousPage, disabled }) => {
  const handlePageChange = (newPage) => {
    setPage(newPage);
  };

  return (
    <div className={cl.pagination}>
      <button
        className={cl.pagination_chevron}
        onClick={previousPage}
        disabled={page === 1 || disabled}
      >
        <FontAwesomeIcon icon={faAngleLeft} />
      </button>
      <button className={[cl.pagination_button, cl.active].join(" ")}>
        {page}
      </button>
      <button
        className={cl.pagination_chevron}
        onClick={nextPage}
        disabled={disabled}
      >
        <FontAwesomeIcon icon={faAngleRight} />
      </button>
    </div>
  );
};

export default SimplePagination;
