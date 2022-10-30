import React from "react";

data = [
  {
    title: "Node1",
    children: [
      {
        title: "Child Node1",
      },
      {
        title: "Child Node2",
      },
    ],
  },
  {
    title: "Node2",
    children: [
      {
        title: "Child Node3",
      },
      {
        title: "Child Node4",
      },
    ],
  },
];

const TreeSelect = ({ data }) => {
  const [selectedItems, setSelectedItems] = useState([]);
  // implement tree select component
  return (
    <div>
      <div>Selected Items: {selectedItems.join(", ")}</div>
      <div>Tree Select Component</div>
    </div>
  );
};

export default TreeSelect;
