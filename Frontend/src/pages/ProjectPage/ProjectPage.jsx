import { useParams } from "react-router-dom";
import cl from "./ProjectPage.module.scss";
import TopNavbar from "../../components/navbars/TopNavbar/TopNavbar";
import { lazy, Suspense, useEffect, useState } from "react";
import { SkeletonFiltersList } from "../../components/loading/SkeletonFiltersList";
import Table from "../../components/dataDisplay/Table/Table";
import Pagination from "../../components/dataDisplay/Pagination/Pagination";

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

  useEffect(() => {
    console.log("fetched", id);
  }, [id]);

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
            <Suspense fallback={<SkeletonFiltersList />}>
              <Filters filters={project.filters} />
            </Suspense>
          </div>
          <div className={cl.table}>
            <h2>Таблица</h2>
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
