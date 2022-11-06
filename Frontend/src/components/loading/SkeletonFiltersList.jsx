import React from "react";
import cl from "./Skeletons.module.scss";

export const SkeletonFiltersList = () => {
  return (
    <div className={cl.filters}>
      <div className={cl.skeleton_card}></div>
      <div className={cl.skeleton_card}></div>
      <div className={cl.skeleton_card}></div>
    </div>
  );
};
