import { useParams } from "react-router-dom";
import cl from "./ProjectPage.module.scss";
import TopNavbar from "../../components/navbars/TopNavbar/TopNavbar";
import { lazy, Suspense, useEffect, useState } from "react";
import { SkeletonFiltersList } from "../../components/loading/SkeletonFiltersList";
import Table from "../../components/dataDisplay/Table/Table";
import Pagination from "../../components/dataDisplay/Pagination/Pagination";
import { useContext } from "react";
import { AuthContext } from "../../api/AuthContext";
import { doUpdateProject } from "../../api/Auth";

const Filters = lazy(() => import("./Filters/Filters"));

const project = {
  id: 1,
  name: "Project 1",
  created_at: "29 октября 14:22",
  updated_at: "29 октября 14:20",
  filters: [
    {
      name: "District",
      value: "All",
    },
    {
      name: "Filter 1",
    },
  ],
};

const ProjectPage = () => {
  const { id } = useParams();
  const [currentPage, setCurrentPage] = useState(1);
  const [isFiltersPage, setIsFiltersPage] = useState(true);
  const { projects, updateProjects } = useContext(AuthContext);
  const [currentProject, setCurrentProject] = useState();
  const [projectSaving, setProjectSaving] = useState(false);

  const onProjectSave = async (filters) => {
    setProjectSaving(true);
    const countriesIds = filters.countries
      ? filters.countries.map((c) => c.id)
      : [];
    const itemsIds = filters.items ? filters.items.map((c) => c.id) : [];
    const regionsIds = filters.regions ? filters.regions.map((c) => c.id) : [];

    console.log(currentProject);
    const newProject = {
      ...currentProject,
      filter: {
        countries: countriesIds,
        itemTypes: itemsIds,
        regions: regionsIds,
      },
    };

    const response = await doUpdateProject(newProject);
    console.log(response);
    setProjectSaving(false);
  };

  useEffect(() => {
    updateProjects();
  }, []);

  useEffect(() => {
    if (projects) {
      const project = projects.find((project) => project.id == id);
      setCurrentProject(project);
    }
  }, [projects, id]);

  return (
    <div className={cl.project_container}>
      <div className={cl.topnav_container}>
        <TopNavbar pageTitle={project.name} />
      </div>
      <main className={cl.main_content}>
        <nav className={cl.mobile_nav}>
          <button
            className={isFiltersPage ? cl.active : ""}
            onClick={() => setIsFiltersPage(true)}
          >
            Фильтры
          </button>
          <button
            className={!isFiltersPage ? cl.active : ""}
            onClick={() => setIsFiltersPage(false)}
          >
            Таблица
          </button>
        </nav>
        <div
          className={[
            cl.project,
            isFiltersPage ? cl.filters_page : cl.table_page,
          ].join(" ")}
        >
          <div className={cl.filters}>
            <h2 className={cl.project_title}>
              {currentProject && currentProject.name}
            </h2>
            <span className={cl.horizontal_separator}></span>
            <h2 className={cl.filters_title}>Фильтры</h2>
            <Suspense fallback={<SkeletonFiltersList />}>
              {currentProject && (
                <Filters
                  filters={project.filters}
                  project={currentProject}
                  onProjectSave={onProjectSave}
                  projectSaving={projectSaving}
                />
              )}
            </Suspense>
          </div>
          <div className={cl.table}>
            <h2 className={cl.table_title}>
              Таблица{" "}
              <span className={cl.helper_text}>
                (обновляется после сохранения)
              </span>
            </h2>
            <div className={cl.table_container}>
              <Table />
            </div>
            <div className={cl.pagination}>
              <Pagination
                page={currentPage}
                setPage={setCurrentPage}
                totalPages={4}
              />
            </div>
          </div>
        </div>
      </main>
    </div>
  );
};

export default ProjectPage;
