import { useState } from "react";
import cl from "./RadioMenu.module.scss";

{
  /* <RadioMenu
buttons={["Фильтры", "Таблица", "Выбранные"]}
onSelect={(index) => setSelectedTab(index)}
/> */
}

const RadioMenu = ({ buttons, onSelect, className }) => {
  const [selected, setSelected] = useState(0);

  const handleClick = (index) => {
    setSelected(index);
    onSelect(index);
  };

  return (
    <nav className={[cl.radio_menu, className].join(" ")}>
      {buttons.map((button, index) => (
        <button
          key={index}
          className={selected === index ? cl.active : ""}
          onClick={() => handleClick(index)}
        >
          {button}
        </button>
      ))}
    </nav>
  );
};

export default RadioMenu;
