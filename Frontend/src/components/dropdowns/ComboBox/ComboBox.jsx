import React, { useEffect, useState } from "react";
import DropdownTreeSelect from "react-dropdown-tree-select";
// import data from "./test.json";
import "react-dropdown-tree-select/dist/styles.css";
import "./ComboBox.scss";

const ItemTreeSelect = ({ data, getNodes }) => {
  const [selectedNodes, setSelectedNodes] = useState([]);

  const onChange = (currentNode, selectedNodes) => {
    setSelectedNodes(selectedNodes);
  };

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
