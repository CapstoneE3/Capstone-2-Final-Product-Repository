import { Container } from "@material-ui/core";
import React from "react";
import { ShoppingList } from "./mockEntries";
import ShoppingListEntry from "./ShoppingListEntry";

interface ShoppingListTabProps {
  activeTab: number;
  index: number;
  propEntries: ShoppingList[];
  handleRemove: (shoppingList: ShoppingList) => void;
  handleAdd: (shoppingList:ShoppingList) => void;
}

const getKey = (name: string) => {
  var key = 0;
  for (let i = 0; i < name.length; i++) {
    key += name.charCodeAt(i);
  }

  return key;
};

const ShoppingListTab: React.FC<ShoppingListTabProps> = ({
  activeTab,
  index,
  propEntries,
  handleRemove,
  handleAdd,
}) => {
  return (
    <>
      {activeTab === index && (
        <Container style={{ maxWidth: "none" }}>
          {propEntries.map((r, i) => {
            return (
              <ShoppingListEntry
                key={getKey(r.name)}
                i={i}
                // recipeID={r.rid}
                // name={r.name}
                category={r.category}
                // intake={r.intake}
                // price={r.price}
                // quantity={r.quantity}
                // img={r.img}
                shoppingList={r}
                handleRemove={handleRemove}
                handleAdd={handleAdd}
              ></ShoppingListEntry>
            );
          })}
        </Container>
      )}
    </>
  );
};

export default ShoppingListTab;
