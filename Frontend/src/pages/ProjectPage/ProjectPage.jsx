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
import SimplePagination from "../../components/dataDisplay/SimplePagination/SimplePagination";
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
  const [tableData, setTableData] = useState([]);
  const [reportCreateDialog, setReportCreateDialog] = useState(false);
  const [reportFilters, setReportFilters] = useState({});
  const [reportDialogDisabled, setReportDialogDisabled] = useState(false);
  const [loadingPage, setLoadingPage] = useState(true);
  const navigate = useNavigate();

  const onProjectSave = async (filters) => {
    setProjectSaving(true);
    const countriesIds = filters.countries
      ? filters.countries.map((c) => c.id)
      : [];
    const itemsIds = filters.items ? filters.items.map((c) => c.id) : [];
    const regionsIds = filters.regions ? filters.regions.map((c) => c.id) : [];

    const newProject = {
      ...currentProject,
      filter: {
        countries: countriesIds,
        itemTypes: itemsIds,
        regions: regionsIds,
      },
    };

    await doUpdateProject(newProject);
    await getTable(filters);
    setProjectSaving(false);
  };

  const onReportDialogOpen = (filters) => {
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
    doCreateReport(title, reportFilters).then((res) => {
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

  const nextPage = () => {
    setCurrentPage(currentPage + 1);
  };

  const previousPage = () => {
    setCurrentPage(currentPage - 1);
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

  useEffect(() => {
    setLoadingPage(true);
    if (currentProject) {
      getTable(currentProject.filter).then(() => {
        setLoadingPage(false);
      });
    }
  }, [currentPage]);

  useEffect(() => {
    if (currentProject) {
      doGetTable(currentProject.filter).then((response) => {
        setTableData(response);
      });
    }
  }, [currentProject]);

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
