import React, { useEffect, useState } from "react";
import DropdownTreeSelect from "react-dropdown-tree-select";
// import data from "./test.json";
import "react-dropdown-tree-select/dist/styles.css";
import "./ComboBox.scss";

const ItemTreeSelect = ({ data }) => {
  const assignObjectPaths = (obj, stack) => {
    Object.keys(obj).forEach((k) => {
      const node = obj[k];
      if (typeof node === "object") {
        node.path = stack ? `${stack}.${k}` : k;
        assignObjectPaths(node, node.path);
      }
    });
  };

  useEffect(() => {
    // for each element with classname "search" change the placeholder text to "Поиск"
    const searchElements = document.getElementsByClassName("search");
    for (let i = 0; i < searchElements.length; i++) {
      searchElements[i].placeholder = "Поиск";
    }
  }, []);

  return (
    <DropdownTreeSelect
      onNodeToggle={onNodeToggle}
      data={treeData}
      onChange={onChange}
      className={"tree-select-container"}
    />
  );
};

export default ItemTreeSelect;
