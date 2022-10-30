import React, { useState } from "react";
import cl from "./Filters.module.scss";
import "antd/lib/tree-select/style/css";
import { TreeSelect } from "antd";
const { SHOW_PARENT } = TreeSelect;

const treeData = [
  {
    title: "Node1",
    value: "0-0",
    key: "0-0",
    children: [
      {
        title: "Child Node1",
        value: "0-0-0",
        key: "0-0-0",
        children: [
          {
            title: "Child Node1",
            value: "0-0-0",
            key: "0-0-0",
            children: [
              {
                title: "Child Node1",
                value: "0-0-0",
                key: "0-0-0",
                children: [
                  {
                    title: "Child Node1",
                    value: "0-0-0",
                    key: "0-0-0",
                  },
                ],
              },
            ],
          },
        ],
      },
    ],
  },
  {
    title: "Node2",
    value: "0-1",
    key: "0-1",
    children: [
      {
        title: "Child Node3Child Node3Child Node3Child Node3Child Node3",
        value: "0-1-0",
        key: "0-1-1",
      },
      {
        title: "Child Node4",
        value: "0-1-1",
        key: "0-1-0",
      },
      {
        title: "Child Node5",
        value: "0-1-2",
        key: "0-1-2",
      },
    ],
  },
];
const Filters = () => {
  const [value, setValue] = useState(["0-0-0"]);

  const onChange = (newValue) => {
    console.log("onChange ", value);
    setValue(newValue);
  };

  const tProps = {
    treeData,
    value,
    onChange,
    treeCheckable: true,
    showCheckedStrategy: SHOW_PARENT,
    placeholder: "Выбор фильтров",
    style: {
      width: "100%",
    },
  };
  return (
    <div className={cl.filters}>
      <p className={cl.filter_title}>Test</p>
      <TreeSelect {...tProps} />
    </div>
  );
};

export default Filters;
