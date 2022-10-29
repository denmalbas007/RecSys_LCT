import cl from "./SignInPage.module.scss";
import logo from "../../assets/logos/logo_full.png";
import { IconInput } from "../../components/inputs/IconInput/IconInput";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEnvelope, faKey } from "@fortawesome/free-solid-svg-icons";
import AccentButton from "../../components/buttons/AccentButton/AccentButton";
import { useState } from "react";
import { doSignIn } from "../../api/Auth";

const SignInPage = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const signIn = (e) => {
    e.preventDefault();
    if (!email || !password) {
      setError("Пожалуйста, заполните все поля");
      return;
    }

    const response = doSignIn(email, password);
    if (response.error) {
      setError(response.error);
      return;
    }
    setError("");
    console.log(response);
  };

  return (
    <div className={cl.signin_container}>
      <nav className={cl.logo_container}>
        <img src={logo} alt="logo" />
      </nav>
      <form className={cl.signin} onSubmit={signIn}>
        <h2>Вход в систему</h2>
        <div className={cl.group}>
          <p>Почта</p>
          <IconInput
            type="email"
            onChange={(e) => setEmail(e.target.value)}
            icon={<FontAwesomeIcon icon={faEnvelope} />}
            placeholder="example@example.com"
          />
        </div>
        <div className={cl.group}>
          <p>Пароль</p>
          <IconInput
            onChange={(e) => setPassword(e.target.value)}
            icon={<FontAwesomeIcon icon={faKey} />}
            placeholder="********"
            type="password"
          />
        </div>
        <div className={cl.error}>{error}</div>
        <AccentButton>Войти в систему</AccentButton>
      </form>
    </div>
  );
};

export default SignInPage;
