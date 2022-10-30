import { useParams } from "react-router-dom";
import cl from "./ProjectPage.module.scss";
import TopNavbar from "../../components/navbars/TopNavbar/TopNavbar";
import { lazy, Suspense, useEffect } from "react";

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
  // const { project, setProject } = useState();
  useEffect(() => {
    console.log("fetched", id);
  }, [id]);

  return (
    <div className={cl.project_container}>
      <div className={cl.topnav_container}>
        <TopNavbar pageTitle={project.name} />
      </div>
      <main className={cl.project}>
        <div className={cl.filters}>
          <Suspense fallback={<div>Loading...</div>}>
            <Filters filters={project.filters} />
          </Suspense>
        </div>
        <div className={cl.table}>Таблица</div>
      </main>
    </div>
  );
};

export default ProjectPage;
