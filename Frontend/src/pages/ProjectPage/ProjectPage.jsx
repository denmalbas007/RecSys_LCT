import cl from "./ProjectPage.module.scss";

const ProjectPage = () => {
  return (
    <div className={cl.project_container}>
      <nav>navbar</nav>
      <main className={cl.content}>
        <div className={cl.filters}>Filters</div>
        <div className={cl.table}>Table</div>
        <div className={cl.selected_items}>Selected Items</div>
      </main>
    </div>
  );
};

export default ProjectPage;
