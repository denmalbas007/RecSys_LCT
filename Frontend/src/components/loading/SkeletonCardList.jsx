import cl from "./Skeletons.module.scss";

export const SkeletonCardList = () => {
  return (
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
  );
};
