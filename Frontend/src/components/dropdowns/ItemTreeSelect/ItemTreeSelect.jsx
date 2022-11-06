import React, { useEffect, useState } from "react";
import DropdownTreeSelect from "react-dropdown-tree-select";
// import data from "./test.json";
import "react-dropdown-tree-select/dist/styles.css";
import "./ItemTreeSelect.scss";

const ItemTreeSelect = ({ data, fetchNode }) => {
  const [treeData, setTreeData] = useState(data);

  const onNodeToggle = async (currentNode, toggled) => {
    const children = await fetchNode(currentNode.id);

    const updatedTree = (tree, children, nodeId, expanded) => {
      return tree.map((node) => {
        if (node.id == nodeId) {
          if (node.children[0].label !== "Загрузка...") {
            return {
              ...node,
              expanded,
            };
          } else {
            return {
              ...node,
              children,
              expanded,
            };
          }
        } else if (node.children) {
          return {
            ...node,
            children: updatedTree(node.children, children, nodeId, expanded),
          };
        }
        return node;
      });
    };
    const updatedData = updatedTree(
      treeData,
      children,
      currentNode.id,
      currentNode.expanded
    );

    setTreeData(updatedData);
  };

  const onChange = (currentNode, selectedNodes) => {
    const updatedTree = (tree, nodeId, checked) => {
      return tree.map((node) => {
        if (node.id == nodeId) {
          return {
            ...node,
            checked: checked,
          };
        } else if (node.children) {
          return {
            ...node,
            children: updatedTree(node.children, nodeId, checked),
          };
        }
        return node;
      });
    };
    const updatedData = updatedTree(
      treeData,
      currentNode.id,
      currentNode.checked
    );

    setTreeData(updatedData);
  };

  useEffect(() => {
    setTreeData(data);
  }, [data]);

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
