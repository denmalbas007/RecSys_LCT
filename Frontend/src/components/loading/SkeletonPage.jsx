import { SkeletonCardList } from "./SkeletonCardList";
import cl from "./Skeletons.module.scss";

export const SkeletonPage = () => {
  return (
    <div className={cl.container}>
      <div className={cl.nav_container}>
        <div className={cl.skeleton_icon}></div>
        <div className={cl.skeleton_text}></div>
        <div className={cl.skeleton_text}></div>
        <div className={cl.skeleton_text}></div>
      </div>
      <SkeletonCardList />
    </div>
  );
};
