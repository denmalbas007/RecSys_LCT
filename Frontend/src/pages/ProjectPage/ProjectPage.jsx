import { useParams } from "react-router-dom";
import cl from "./ProjectPage.module.scss";
import TopNavbar from "../../components/navbars/TopNavbar/TopNavbar";
import { lazy, Suspense, useEffect, useState } from "react";
import { SkeletonFiltersList } from "../../components/loading/SkeletonFiltersList";
import Table from "../../components/dataDisplay/Table/Table";
import { useContext } from "react";
import { AuthContext } from "../../api/AuthContext";
import { doCreateReport, doGetTable, doUpdateProject } from "../../api/Auth";
import CreateReportDialog from "../../components/dialogs/CreateReportDialog/CreateReportDialog";
import { useNavigate } from "react-router-dom";
// import SimplePagination from "../../components/dataDisplay/SimplePagination/SimplePagination";
const Filters = lazy(() => import("./Filters/Filters"));

const ProjectPage = () => {
  const { id } = useParams();
  const [isFiltersPage, setIsFiltersPage] = useState(true);
  const { projects, updateProjects } = useContext(AuthContext);
  const [currentProject, setCurrentProject] = useState();
  const [projectSaving, setProjectSaving] = useState(false);
  const [tableData, setTableData] = useState([]);
  const [reportCreateDialog, setReportCreateDialog] = useState(false);
  const [reportFilters, setReportFilters] = useState({});
  const [reportDialogDisabled, setReportDialogDisabled] = useState(false);
  // const [currentPage, setCurrentPage] = useState(1);
  // const [loadingPage, setLoadingPage] = useState(true);
  const navigate = useNavigate();

  const onProjectSave = (filters) => {
    console.log(filters);
    setProjectSaving(true);

    // get ID of every filter
    const countriesIds = filters.countries
      ? filters.countries.map((c) => c.id)
      : [];
    const itemsIds = filters.items ? filters.items.map((c) => c.id) : [];
    const regionsIds = filters.regions ? filters.regions.map((c) => c.id) : [];
    // getTable(filters).then(() => {
    setProjectSaving(false);
    // });

    const newProject = {
      ...currentProject,
      filter: {
        countries: countriesIds,
        itemTypes: itemsIds,
        regions: regionsIds,
      },
    };
    doUpdateProject(newProject);
  };

  const onReportDialogOpen = (filters) => {
    // get ID of every filter
    const countriesIds = filters.countries
      ? filters.countries.map((c) => c.id)
      : [];
    const itemsIds = filters.items ? filters.items.map((c) => c.id) : [];
    const regionsIds = filters.regions ? filters.regions.map((c) => c.id) : [];
    setReportFilters({
      countries: countriesIds,
      itemTypes: itemsIds,
      regions: regionsIds,
    });

    setReportCreateDialog(true);
  };

  const onReportCreate = (title) => {
    setReportDialogDisabled(true);
    doCreateReport(title, reportFilters).then(() => {
      setReportCreateDialog(false);
      setReportDialogDisabled(false);
      navigate("/reports");
    });
  };

  const getTable = async (filters) => {
    const countriesIds = filters.countries
      ? filters.countries.map((c) => c.id)
      : [];
    const itemsIds = filters.items ? filters.items.map((c) => c.id) : [];
    const regionsIds = filters.regions ? filters.regions.map((c) => c.id) : [];

    const response = await doGetTable({
      countries: countriesIds,
      itemTypes: itemsIds,
      regions: regionsIds,
    });

    setTableData(response);
  };

  useEffect(() => {
    updateProjects();
  }, []);

  useEffect(() => {
    if (currentProject) {
      doGetTable(currentProject.filter).then((response) => {
        setTableData(response);
      });
    }
  }, [currentProject]);

  useEffect(() => {
    if (projects) {
      const project = projects.find((project) => project.id == id);
      setCurrentProject(project);
    }
  }, [projects, id]);

  // const nextPage = () => {
  //   setCurrentPage(currentPage + 1);
  // };

  // const previousPage = () => {
  //   setCurrentPage(currentPage - 1);
  // };
  // useEffect(() => {
  //   setLoadingPage(true);
  //   if (currentProject) {
  //     getTable(currentProject.filter).then(() => {
  //       setLoadingPage(false);
  //     });
  //   }
  // }, [currentPage]);

  return (
    <div className={cl.project_container}>
      {reportCreateDialog && (
        <CreateReportDialog
          onCreate={onReportCreate}
          error={""}
          onCancel={() => setReportCreateDialog(false)}
          disabled={reportDialogDisabled}
        />
      )}
      <div className={cl.topnav_container}>
        <TopNavbar />
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
                  project={currentProject}
                  onProjectSave={onProjectSave}
                  onReportCreate={onReportDialogOpen}
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
              <Table data={tableData} />
            </div>
            {/* <div className={cl.pagination}>
              <SimplePagination
                page={currentPage}
                nextPage={() => setCurrentPage(currentPage + 1)}
                prevPage={() => setCurrentPage(currentPage - 1)}
                disabled={loadingPage}
              />
            </div> */}
          </div>
        </div>
      </main>
    </div>
  );
};

export default ProjectPage;
