import cl from "./ProjectCard.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faClockRotateLeft } from "@fortawesome/free-solid-svg-icons";

const ProjectCard = ({ project }) => {
  return (
    <div className={cl.project_card}>
      <div className={cl.project_header}>
        <div className={cl.project_title}>
          <h3>{project.name}</h3>
        </div>
        <div className={cl.project_date}>
          <p>{project.updated_at}</p>
          <FontAwesomeIcon icon={faClockRotateLeft} />
        </div>
      </div>
      <div className={cl.project_description}>
        <div className={cl.col}>
          <h4 className={cl.header}>Фильтры:</h4>
          <div className={cl.content}>
            {project.filters.slice(0, 3).map((filter, index) => (
              <>
                <p className={cl.item} key={index}>
                  {filter.name.length > 15
                    ? filter.name.slice(0, 15) + "..."
                    : filter.name}
                </p>
                <span className={cl.horizontal_separator} />
              </>
            ))}
          </div>
          {project.filters.length > 3 && (
            <p className={cl.footer}>Еще +{project.filters.length - 3}</p>
          )}
        </div>
        <span className={cl.separator} />
        <div className={cl.col}>
          <h4 className={cl.header}>Элементы:</h4>
          <div className={cl.content}>
            {project.items.slice(0, 3).map((item, index) => (
              <>
                <p className={cl.item} key={index}>
                  {item.name.length > 14
                    ? item.name.slice(0, 15) + "..."
                    : item.name}
                </p>
                <span className={cl.horizontal_separator} />
              </>
            ))}
          </div>
          {project.items.length > 3 && (
            <p className={cl.footer}>Еще +{project.items.length - 3}</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProjectCard;
