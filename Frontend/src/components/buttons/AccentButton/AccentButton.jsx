import cl from "./AccentButton.module.scss";

const AccentButton = ({ disabled, children, ...props }) => {
  return (
    <button
      className={[cl.button, disabled ? cl.disabled : ""].join(" ")}
      {...props}
    >
      {children}
    </button>
  );
};

export default AccentButton;
