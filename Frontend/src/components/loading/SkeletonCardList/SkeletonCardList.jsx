import cl from "./SkeletonCardList.module.scss";

const SkeletonCardList = () => {
  return (
    <div className={cl.skeleton_card_list}>
      <div className={cl.grid}>
        <div className={cl.skeleton_card}></div>
        <div className={cl.skeleton_card}></div>
        <div className={cl.skeleton_card}></div>
        <div className={cl.skeleton_card}></div>
        <div className={cl.skeleton_card}></div>
      </div>
    </div>
  );
};

export default SkeletonCardList;
