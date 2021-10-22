import {
  Avatar,
  Button,
  ButtonGroup,
  Card,
  Container,
  createStyles,
  IconButton,
  ListItem,
  ListItemAvatar,
  ListItemText,
  makeStyles,
  styled,
  Theme,
  useTheme,
} from "@material-ui/core";
import {
  AddShoppingCart,
  Block,
  Edit,
  Favorite,
  FavoriteBorderOutlined,
  ImportContacts,
} from "@material-ui/icons";
import React from "react";
import { Recipe } from "./mockEntries";

// const useStyles = makeStyles((theme:Theme) => createStyles({

// }))

export const StyledActionButton = styled(Button)(({ theme }) => ({
  width: "100%",
  color: theme.palette.background.default,
  backgroundColor: theme.palette.background.default,
  "&:hover": {
    opacity: 0.7,
  },
}));

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    cardContainer: {
      marginTop: theme.spacing(2),
      marginBottom: theme.spacing(2),
    },

    listItemContainer: {
      display: "flex",
      flexDirection: "row",
      justifyContent: "space-around",
      [theme.breakpoints.down("sm")]: {
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        paddingBottom: 0,
        paddingInline: 0,
      },
    },

    entryInfoContainer: {
      display: "flex",
      justifyContent: "flex-start",
      alignItems: "center",
      [theme.breakpoints.up("md")]: {
        paddingLeft: 0,
      },
    },

    entryAvatar: {
      aspectRatio: "1.35",
      minWidth: theme.spacing(10),
      height: "100%",
      outline: `3px solid ${theme.palette.secondary.main}`,
      margin: `${theme.spacing(2)}px 0px`,
    },

    actionButtonGroup: {
      [theme.breakpoints.up("md")]: {
        width: "70%",
      },
    },

    favoritedButton: {
      color: "#FF6A85",
      "&:hover": {
        color: "#FF6A85",
        opacity: 0.7,
      },
    },

    editButton: {
      backgroundColor: theme.palette.secondary.main,
      "&:hover": {
        backgroundColor: theme.palette.secondary.main,
      },
      [theme.breakpoints.down("xs")]: {
        borderRadius: 0,
      },
    },

    addButton: {
      backgroundColor: theme.palette.primary.dark,
      "&:hover": {
        backgroundColor: theme.palette.primary.dark,
      },
    },

    removeButton: {
      backgroundColor: theme.palette.error.main,

      "&:hover": {
        backgroundColor: theme.palette.error.main,
      },
    },

    openButton: {
      backgroundColor: theme.palette.text.primary,
      [theme.breakpoints.down("xs")]: {
        borderRadius: 0,
      },
    },
  })
);

interface RecipeEntryProps {
  recipe: Recipe;
  handleOpenEdit: (recipe: Recipe, i: number) => void;
  handleRemove: (recipe: Recipe) => void;
  handleAdd: () => void;
  i: number;
}

const RecipeEntry: React.FC<RecipeEntryProps> = ({
  recipe,
  handleOpenEdit,
  handleRemove,
  handleAdd,
  i,
}) => {
  const theme = useTheme();
  const classes = useStyles(theme);

  const ingredientsString = getIngredientsString(recipe.ingredients);

  const [isFavorite, setIsFavorite] = React.useState(recipe.fav);

  const handleEditButtonClick = () => {
    handleOpenEdit(recipe, i);
  };

  const handleRemoveButtonClick = () => {
    handleRemove(recipe);
  };

  return (
    <Card elevation={3} classes={{ root: classes.cardContainer }}>
      <ListItem classes={{ root: classes.listItemContainer }}>
        <Container classes={{ root: classes.entryInfoContainer }}>
          <ListItemAvatar>
            <Avatar
              variant="rounded"
              src={recipe.img}
              classes={{ root: classes.entryAvatar }}
            />
          </ListItemAvatar>
          <ListItemText
            primary={
              <Container
                style={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "flex-start",
                }}
              >
                {recipe.name}
                <IconButton
                  onClick={() => setIsFavorite((prev) => !prev)}
                  size="small"
                >
                  {isFavorite ? (
                    <Favorite className={classes.favoritedButton} />
                  ) : (
                    <FavoriteBorderOutlined
                      style={{ color: theme.palette.text.secondary }}
                    />
                  )}
                </IconButton>
              </Container>
            }
            secondary={<Container>{ingredientsString}</Container>}
          />
        </Container>

        <ButtonGroup
          variant="contained"
          size="large"
          fullWidth
          className={classes.actionButtonGroup}
        >
          <StyledActionButton
            className={classes.editButton}
            onClick={handleEditButtonClick}
          >
            <Edit />
          </StyledActionButton>
          <StyledActionButton className={classes.addButton}>
            <AddShoppingCart />
          </StyledActionButton>
          <StyledActionButton
            className={classes.removeButton}
            onClick={handleRemoveButtonClick}
          >
            <Block />
          </StyledActionButton>
          <StyledActionButton className={classes.openButton}>
            <ImportContacts />
          </StyledActionButton>
        </ButtonGroup>
      </ListItem>
    </Card>
  );
};

const getIngredientsString = (ingredients: string[]) => {
  var igstr = "";

  var itersCount = 5;

  var plusOthers = true;

  if (ingredients.length <= 5) {
    itersCount = ingredients.length;
    plusOthers = false;
  }

  for (let i = 0; i < itersCount; i++) {
    igstr += ingredients[i];
    if (i + 1 < itersCount) igstr += ", ";
  }

  if (plusOthers) {
    igstr += ` + ${ingredients.length - 5} more`;
  }

  return igstr;
};

export default RecipeEntry;
