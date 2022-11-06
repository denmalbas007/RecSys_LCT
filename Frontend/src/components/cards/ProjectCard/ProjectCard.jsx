import cl from "./ProjectCard.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faClockRotateLeft } from "@fortawesome/free-solid-svg-icons";
import { useNavigate } from "react-router-dom";

const ProjectCard = (project) => {
  const navigate = useNavigate();

  const reformatDate = (date) =>
    new Date(date).toLocaleString("ru", {
      day: "numeric",
      month: "long",
      hour: "numeric",
      minute: "numeric",
    });

  return (
    <div
      className={cl.project_card}
      onClick={() => navigate(`/project/${project.id}`)}
    >
      <div className={cl.project_header}>
        <div className={cl.project_title}>
          <h3>{project.name}</h3>
        </div>
        <div className={cl.project_date}>
          <p>{reformatDate(project.lastUpdate)}</p>
          <FontAwesomeIcon icon={faClockRotateLeft} />
        </div>
      </div>
      <div className={cl.project_description}>
        <div className={cl.col}>
          <h4 className={cl.header}>Фильтры:</h4>
          <div className={cl.content}>
            <div className={cl.item}>
              <p>Регионы</p>
              <span className={cl.horizontal_separator} />
            </div>
            <div className={cl.item}>
              <p>Товары</p>
              <span className={cl.horizontal_separator} />
            </div>
            <div className={cl.item}>
              <p>Страны</p>
              <span className={cl.horizontal_separator} />
            </div>
          </div>
        </div>
        <span className={cl.separator} />
        <div className={cl.col}>
          <h4 className={cl.header}>Значения:</h4>
          <div className={cl.content}>
            {project.filters.regions.length > 0 ? (
              <div className={cl.item}>
                <p>{project.filters.regions.join(", ")}</p>
                <span className={cl.horizontal_separator} />
              </div>
            ) : (
              <div className={cl.item}>
                <p>Не выбрано</p>
                <span className={cl.horizontal_separator} />
              </div>
            )}
            {project.filters.itemTypes.length > 0 ? (
              <div className={cl.item}>
                <p>{project.filters.itemTypes.join(", ")}</p>
                <span className={cl.horizontal_separator} />
              </div>
            ) : (
              <div className={cl.item}>
                <p>Не выбрано</p>
                <span className={cl.horizontal_separator} />
              </div>
            )}
            {project.filters.countries.length > 0 ? (
              <div className={cl.item}>
                <p>{project.filters.countries.join(", ")}</p>
                <span className={cl.horizontal_separator} />
              </div>
            ) : (
              <div className={cl.item}>
                <p>Не выбрано</p>
                <span className={cl.horizontal_separator} />
              </div>
            )}
            {/* {project.filters &&
              project.filters.slice(0, 3).map((item, index) => (
                <div className={cl.item} key={index}>
                  <p>{item.values.join(", ")}</p>
                  <span className={cl.horizontal_separator} />
                </div>
              ))} */}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProjectCard;
