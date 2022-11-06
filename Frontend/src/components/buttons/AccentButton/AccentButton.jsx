import cl from "./AccentButton.module.scss";

const AccentButton = ({ secondary, disabled, children, ...props }) => {
  return (
    <button
      className={[
        cl.button,
        disabled ? cl.disabled : "",
        secondary ? cl.secondary : "",
      ].join(" ")}
      {...props}
    >
      {children}
    </button>
  );
};

export default AccentButton;
