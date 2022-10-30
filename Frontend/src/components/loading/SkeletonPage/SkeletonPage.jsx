import cl from "./SkeletonPage.module.scss";

const SkeletonPage = () => {
  return (
    <div className={cl.container}>
      <div className={cl.nav_container}>
        <div className={cl.skeleton_icon}></div>
        <div className={cl.skeleton_text}></div>
        <div className={cl.skeleton_text}></div>
        <div className={cl.skeleton_text}></div>
      </div>
      <div className={cl.main_container}>
        <div className={cl.grid}>
          <div className={cl.skeleton_card}></div>
          <div className={cl.skeleton_card}></div>
          <div className={cl.skeleton_card}></div>
          <div className={cl.skeleton_card}></div>
          <div className={cl.skeleton_card}></div>
          <div className={cl.skeleton_card}></div>
          <div className={cl.skeleton_card}></div>
          <div className={cl.skeleton_card}></div>
        </div>
      </div>
    </div>
  );
};

export default SkeletonPage;
