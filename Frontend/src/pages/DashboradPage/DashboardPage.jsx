import ProjectCard from "../../components/cards/ProjectCard/ProjectCard";
import cl from "./DashboardPage.module.scss";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import TopNavbar from "../../components/navbars/TopNavbar/TopNavbar";

const projectsExample = {
  id: 1,
  name: "Project 1",
  created_at: "29 октября 14:22",
  updated_at: "29 октября 14:20",
  filters: [
    {
      name: "Filter Filter Filter Filter Filter Filter",
    },
    {
      name: "Filter 1",
    },
    {
      name: "Filter 1",
    },
    {
      name: "Filter 1",
    },
    {
      name: "Filter 1",
    },
  ],
  items: [
    {
      name: "Item Item Item Item Item Item",
    },
    {
      name: "Item 2",
    },
    {
      name: "Item 2",
    },
    {
      name: "Item 2",
    },
    {
      name: "Item 2",
    },
  ],
};

const DashboardPage = () => {
  return (
    <div className={cl.dashboard_container}>
      <div className={cl.topnav_container}>
        <TopNavbar pageTitle="Проекты" />
      </div>
      <main className={cl.content}>
        <div className={cl.projects_container}>
          <div className={cl.new_project}>
            <FontAwesomeIcon icon={faPlus} />
            <h2>Создать проект</h2>
          </div>
          <ProjectCard project={projectsExample} />
          <ProjectCard project={projectsExample} />
          <ProjectCard project={projectsExample} />
          <ProjectCard project={projectsExample} />
          <ProjectCard project={projectsExample} />

          {/* {projectsExample.map((project) => (
            <ProjectCard project={project} />
          ))} */}
        </div>
      </main>
    </div>
  );
};

export default DashboardPage;
