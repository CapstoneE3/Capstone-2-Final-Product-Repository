import React, { useReducer, useEffect } from 'react';
import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';

import TextField from '@material-ui/core/TextField';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import CardActions from '@material-ui/core/CardActions';
import CardHeader from '@material-ui/core/CardHeader';
import Button from '@material-ui/core/Button';
import Divider from '@material-ui/core/Divider';
import Checkbox from '@material-ui/core/Checkbox';
import Grid from '@material-ui/core/Grid';
import Link from '@material-ui/core/Link';
import Avatar from '@material-ui/core/Avatar';
import AccountCircleIcon from '@material-ui/icons/AccountCircle';
import Typography from '@material-ui/core/Typography';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    container: {
      display: 'flex',
      flexWrap: 'wrap',
      width: 400,
      height: 500,
      margin: `${theme.spacing(0)} auto`
    },
    loginBtn: {
      marginTop: theme.spacing(2),
      flexGrow: 1
    },
    header: {
      textAlign: 'center',
      background: '#50C878',
      color: '#fff'
    },
    card: {
      marginTop: theme.spacing(10)
    }, flex: {
        display: "flex",
        justifyContent: "space-around",
        alignItems: "center"
    }, avatar: {
        margin: theme.spacing(1),
        backgroundColor: theme.palette.secondary.main,
    }
  })
);

//state type

type State = {
  username: string
  password:  string
  isButtonDisabled: boolean
  helperText: string
  isError: boolean
};

const initialState:State = {
  username: '',
  password: '',
  isButtonDisabled: true,
  helperText: '',
  isError: false
};

type Action = { type: 'setUsername', payload: string }
  | { type: 'setPassword', payload: string }
  | { type: 'setIsButtonDisabled', payload: boolean }
  | { type: 'loginSuccess', payload: string }
  | { type: 'loginFailed', payload: string }
  | { type: 'setIsError', payload: boolean };

const reducer = (state: State, action: Action): State => {
  switch (action.type) {
    case 'setUsername': 
      return {
        ...state,
        username: action.payload
      };
    case 'setPassword': 
      return {
        ...state,
        password: action.payload
      };
    case 'setIsButtonDisabled': 
      return {
        ...state,
        isButtonDisabled: action.payload
      };
    case 'loginSuccess': 
      return {
        ...state,
        helperText: action.payload,
        isError: false
      };
    case 'loginFailed': 
      return {
        ...state,
        helperText: action.payload,
        isError: true
      };
    case 'setIsError': 
      return {
        ...state,
        isError: action.payload
      };
  }
}

const SignUp = () => {
  const classes = useStyles();
  const [state, dispatch] = useReducer(reducer, initialState);

  useEffect(() => {
    if (state.username.trim() && state.password.trim()) {
     dispatch({
       type: 'setIsButtonDisabled',
       payload: false
     });
    } else {
      dispatch({
        type: 'setIsButtonDisabled',
        payload: true
      });
    }
  }, [state.username, state.password]);

  const handleLogin = () => {
    if (state.username === 'abc@email.com' && state.password === 'password') {
      dispatch({
        type: 'loginSuccess',
        payload: 'Login Successfully'
      });
    } else {
      dispatch({
        type: 'loginFailed',
        payload: 'Incorrect username or password'
      });
    }
  };

  const handleKeyPress = (event: React.KeyboardEvent) => {
    if (event.keyCode === 13 || event.which === 13) {
      state.isButtonDisabled || handleLogin();
    }
  };

  const handleUsernameChange: React.ChangeEventHandler<HTMLInputElement> =
    (event) => {
      dispatch({
        type: 'setUsername',
        payload: event.target.value
      });
    };

  const handlePasswordChange: React.ChangeEventHandler<HTMLInputElement> =
    (event) => {
      dispatch({
        type: 'setPassword',
        payload: event.target.value
      });
    }

  const [checked, setChecked] = React.useState(true);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setChecked(event.target.checked);
  };


  return (
    
    <React.Fragment>

        <Typography variant='h4' > Handy Pantry </Typography>
        <form className={classes.container} noValidate autoComplete="off" >
            
        <Card className={classes.card} >
            <CardHeader className={classes.header} title="Create Account" />
            <CardContent>
            <div>
                <div className={classes.flex}>
                <TextField
                error={state.isError}
                
                id="firstname"
                type="email"
                label="First Name"
                placeholder="First Name"
                margin="normal"
                variant="outlined"
                onChange={handleUsernameChange}
                onKeyPress={handleKeyPress}
                />
                <Divider  variant="middle"/>
                <TextField
                error={state.isError}
                
                id="lastname"
                type="email"
                label="Last Name"
                placeholder="Last Name"
                margin="normal"
                variant="outlined"
                onChange={handleUsernameChange}
                onKeyPress={handleKeyPress}
                />

                </div>
                <TextField
                error={state.isError}
                fullWidth
                id="userEmail"
                type="email"
                label="Email"
                placeholder="Email"
                variant="outlined"
                margin="normal"
                onChange={handleUsernameChange}
                onKeyPress={handleKeyPress}
                />
                <TextField
                error={state.isError}
                fullWidth
                id="password"
                type="password"
                label="Password"
                placeholder="Password"
                margin="normal"
                variant="outlined"
                helperText={state.helperText}
                onChange={handlePasswordChange}
                onKeyPress={handleKeyPress}
                />
            </div>
            </CardContent>
        
            <div>
                <Checkbox
                checked={checked}
                onChange={handleChange}
                inputProps={{ 'aria-label': 'primary checkbox' }}
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
                disabled={state.isButtonDisabled}>
                Sign Up
            </Button>
            </CardActions>
        </Card>
        <Grid container justifyContent="flex-end">
                <Grid item>
                <Link href="#" variant="body2">
                    Already have an account? Sign in
                </Link>
                </Grid>
            </Grid>
        </form>

    </React.Fragment>
  );
}

export default SignUp;