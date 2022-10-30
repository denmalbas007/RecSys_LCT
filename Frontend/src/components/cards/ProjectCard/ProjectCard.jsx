import cl from "./ProjectCard.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faClockRotateLeft } from "@fortawesome/free-solid-svg-icons";
import { useNavigate } from "react-router-dom";

const ProjectCard = ({ project }) => {
  // on click navigate to /project/:id where id is project.id
  const navigate = useNavigate();

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
          <p>{project.updated_at}</p>
          <FontAwesomeIcon icon={faClockRotateLeft} />
        </div>
      </div>
      <div className={cl.project_description}>
        <div className={cl.col}>
          <h4 className={cl.header}>Фильтры:</h4>
          <div className={cl.content}>
            {project.filters.slice(0, 3).map((filter, index) => (
              <div className={cl.item} key={index}>
                <p>{filter.name}</p>
                <span className={cl.horizontal_separator} />
              </div>
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
              <div className={cl.item} key={index}>
                <p>{item.name}</p>
                <span className={cl.horizontal_separator} />
              </div>
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
