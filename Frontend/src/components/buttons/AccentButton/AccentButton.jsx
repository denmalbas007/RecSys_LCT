import cl from "./AccentButton.module.scss";

const AccentButton = ({ children, ...props }) => {
  return (
    <button className={cl.button} {...props}>
      {children}
    </button>
  );
};

export default AccentButton;
