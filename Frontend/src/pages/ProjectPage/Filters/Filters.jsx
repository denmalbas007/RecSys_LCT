import React, { useState } from "react";
import RadioMenu from "../../../components/buttons/RadioMenu/RadioMenu";
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
      <h2>Фильтры</h2>
      <div className={cl.filter}>
        <p className={cl.filter_title}>Направление</p>
        <RadioMenu fontSize={16} buttons={["Импорт", "Экспорт"]} />
      </div>
      <div className={cl.filter}>
        <p className={cl.filter_title}>Страна</p>
        <TreeSelect />
      </div>
      <div className={cl.filter}>
        <p className={cl.filter_title}>Регион</p>
        <TreeSelect />
      </div>
      <div className={cl.filter}>
        <p className={cl.filter_title}>Категория</p>
        <TreeSelect />
      </div>
    </div>
  );
};

export default Filters;
