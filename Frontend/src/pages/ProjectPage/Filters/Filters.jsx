import React, { useState } from "react";
import TreeSelect from "../../../components/dropdowns/TreeSelect/TreeSelect";
import cl from "./Filters.module.scss";

const Filters = () => {
  const [value, setValue] = useState([]);

  const onChange = (newValue) => {
    console.log("onChange ", value);
    setValue(newValue);
  };

  return (
    <div className={cl.filters}>
      <TreeSelect />
    </div>
  );
};

export default Filters;
