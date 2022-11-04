import cl from "./StandartInput.module.scss";

const StandartInput = ({ ...props }) => {
  return <input className={cl.input} {...props} />;
};

export default StandartInput;
