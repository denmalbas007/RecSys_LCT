import cl from "./IconInput.module.scss";
import { useState } from "react";
import { useEffect } from "react";

export const IconInput = ({ error, resolveError, icon, ...props }) => {
  const [isFocused, setIsFocused] = useState(false);
  const [isError, setIsError] = useState(error);

  useEffect(() => {
    setIsError(error);
  }, [error]);

  return (
    <div
      className={[
        cl.container,
        isFocused ? cl.focused : "",
        isError ? cl.error : "",
      ].join(" ")}
    >
      <span className={cl.icon}>{icon}</span>
      <input
        className={cl.input}
        onFocus={() => {
          setIsFocused(true);
          resolveError && resolveError();
        }}
        onBlur={() => setIsFocused(false)}
        {...props}
      />
    </div>
  );
};
