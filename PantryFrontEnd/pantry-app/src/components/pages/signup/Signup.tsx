import Button from "@material-ui/core/Button";
import Card from "@material-ui/core/Card";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import CardHeader from "@material-ui/core/CardHeader";
import Checkbox from "@material-ui/core/Checkbox";
import Divider from "@material-ui/core/Divider";
import Grid from "@material-ui/core/Grid";
import Link from "@material-ui/core/Link";
import { createStyles, makeStyles, Theme } from "@material-ui/core/styles";
import TextField from "@material-ui/core/TextField";
import Typography from "@material-ui/core/Typography";
import React, { useState } from "react";
import { Redirect, Switch } from "react-router-dom";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    container: {
      display: "flex",
      flexWrap: "wrap",
      width: 400,
      height: 500,
      margin: `${theme.spacing(0)} auto`,
    },
    loginBtn: {
      marginTop: theme.spacing(2),
      flexGrow: 1,
    },
    header: {
      textAlign: "center",
      background: "#50C878",
      color: "#fff",
    },
    card: {
      marginTop: theme.spacing(10),
    },
    flex: {
      display: "flex",
      justifyContent: "space-around",
      alignItems: "center",
    },
    avatar: {
      margin: theme.spacing(1),
      backgroundColor: theme.palette.secondary.main,
    },
  })
);

const SignUp = () => {
  const classes = useStyles();
  // const history = useHistory();

  const [state, setState] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    helperText: "",
    isButtonDisabled: true,
    isError: false,
    checkMe: false,
  });

  // const [message, setMessage] = React.useState("");

  const handleLogin = async () => {
    // I've done the sign-up, please do the login now
    // It should be very similar, once they've logged in,
    // redirect to inventory with push - Evin
    const params = {
      email: state.email,
      password: state.password,
      firstName: state.firstName,
      lastName: state.lastName,
    };
    const response = await fetch(
      "https://pantties.azurewebsites.net/api/Users/Register?",
      {
        method: "POST",
        mode: "cors",
        body: JSON.stringify(params),
        headers: { "Content-Type": "application/json" },
      }
    );

    // const data = await response.json();

    if (response.ok) {
      // history.goBack();
      setToLogin(true);
    }
    // console.log(data.message);
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const name = event.target.name;
    const value =
      event.target.type === "checkbox"
        ? event.target.checked
        : event.target.value;
    setState({
      ...state,
      [name]: value,
    });
  };

  const handleKeyPress = (event: React.KeyboardEvent) => {
    // if (event.keyCode === 13 || event.which === 13) {
    //   state.isButtonDisabled || handleLogin();
    // }
    // handleLogin();
  };

  const [checked, setChecked] = React.useState(true);

  const [toLogin, setToLogin] = React.useState(false);
  //Handling change for the checkbox
  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setChecked(event.target.checked);
  };

  const handleRoute = () => {
    // history.goBack();
    setToLogin(true);
  };

  return (
    <React.Fragment>
      <Typography variant="h4"> Handy Pantry </Typography>
      <form className={classes.container} noValidate autoComplete="off">
        <Card className={classes.card}>
          <CardHeader className={classes.header} title="Create Account" />
          <CardContent>
            <div>
              <div className={classes.flex}>
                <TextField
                  error={state.isError}
                  name="firstName"
                  id="firstName"
                  label="First Name"
                  placeholder="First Name"
                  margin="normal"
                  variant="outlined"
                  value={state.firstName}
                  onChange={handleInputChange}
                  onKeyPress={handleKeyPress}
                />
                <Divider variant="middle" />
                <TextField
                  error={state.isError}
                  name="lastName"
                  id="lastName"
                  label="Last Name"
                  placeholder="Last Name"
                  margin="normal"
                  variant="outlined"
                  value={state.lastName}
                  onChange={handleInputChange}
                  onKeyPress={handleKeyPress}
                />
              </div>
              <TextField
                error={state.isError}
                fullWidth
                name="email"
                id="email"
                type="email"
                label="Email"
                placeholder="Email"
                variant="outlined"
                margin="normal"
                value={state.email}
                onChange={handleInputChange}
                onKeyPress={handleKeyPress}
              />
              <TextField
                error={state.isError}
                fullWidth
                name="password"
                id="password"
                type="password"
                label="Password"
                placeholder="Password"
                margin="normal"
                variant="outlined"
                value={state.password}
                helperText={state.helperText}
                onChange={handleInputChange}
                onKeyPress={handleKeyPress}
              />
            </div>
          </CardContent>

          <div>
            <Checkbox
              name="checkMe"
              checked={checked}
              onChange={handleChange}
              inputProps={{ "aria-label": "primary checkbox" }}
            />
            I have read and agree to Terms & Conditions
          </div>

          <CardActions>
            <Button
              variant="contained"
              size="large"
              color="secondary"
              className={classes.loginBtn}
              onClick={handleLogin}
              // disabled={state.isButtonDisabled}
            >
              Sign Up
            </Button>
          </CardActions>
        </Card>
        <Grid container justifyContent="flex-end">
          <Grid item>
            <Link component="button" variant="body2" onClick={handleRoute}>
              Already have an account? Sign in
            </Link>
          </Grid>
        </Grid>
      </form>
      <Switch>{toLogin && <Redirect to="/login"></Redirect>}</Switch>
    </React.Fragment>
  );
};

export default SignUp;
