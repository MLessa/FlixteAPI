import React, { Component } from "react";
import {
  View,
  TextInput,
  StyleSheet,
  TouchableOpacity,
  Text,
  ActivityIndicator,
  Image
} from "react-native";
import Icon from "react-native-vector-icons/Feather";
import images from "../images";

export default class TextField extends Component {
  setNativeProps = nativeProps => {
    this._root.setNativeProps(nativeProps);
  };

  constructor(props) {
    super(props);

    this.state = {
      fieldVal: "",
      leftPadding: 0,
      isTextVisible: true,
      secure: false,
      validationMessage: null
    };
  }

  componentDidMount() {
    let { secure, showSearchIcon } = this.props;
    if (secure) {
      this.setState({ secure: true });
      this.setState({ isTextVisible: false });
      this.setState({ leftPadding: 13 });
    }
    if (showSearchIcon) {
      this.setState({ leftPadding: 13 });
    }
  }

  renderSearchIcon = () => {
    let { showSearchIcon } = this.props;

    if (showSearchIcon) {
      return (
        <View style={styles.seePassword}>
          <Image
            style={{
              width: 26,
              height: 26,
              tintColor: "#d46f02",
              marginBottom: 25,
              marginTop: 10,
              left: -10
            }}
            source={images.footer.search}
          />
        </View>
      );
    }
    return null;
  };

  renderViewPassword = () => {
    if (this.state.secure) {
      return (
        <View style={styles.seePassword}>
          <TouchableOpacity
            onPress={() => {
              this.setState({ isTextVisible: !this.state.isTextVisible });
            }}
          >
            <Icon
              style={styles.seePasswordIcon}
              name={this.state.isTextVisible ? "eye" : "eye-off"}
              size={20}
              color="#000"
            />
          </TouchableOpacity>
        </View>
      );
    }
    return null;
  };

  renderValidationMessage = externalValidationMessage => {
    if (this.state.validationMessage || externalValidationMessage) {
      return (
        <View>
          <Text
            style={[
              styles.validationMessage,
              this.state.secure ? { left: 13 } : {}
            ]}
          >
            {externalValidationMessage || this.state.validationMessage}
          </Text>
        </View>
      );
    }
  };

  render() {
    let {
      width,
      onUpdate,
      placeholder,
      style,
      validationFunction,
      showLoader,
      externalValidationMessage
    } = this.props;

    return (
      <View style={styles.textInputMain}>
        <View style={styles.textInputContainer}>
          <TextInput
            style={[
              styles.content,
              style,
              { width: width },
              { left: this.state.leftPadding },
              showLoader ? { left: 12 } : {}
            ]}
            secureTextEntry={!this.state.isTextVisible}
            onChangeText={text => {
              this.setState({ fieldVal: text });
              if (validationFunction)
                this.setState({ validationMessage: validationFunction(text) });
              onUpdate(text);
            }}
            value={this.state.fieldVal}
            placeholder={placeholder}
            placeholderTextColor="#444444"
          />
          {this.renderSearchIcon()}

          {this.renderViewPassword()}
          {showLoader ? (
            <ActivityIndicator
              style={styles.loader}
              size="small"
              color="#d46f02"
            />
          ) : null}
        </View>
        {this.renderValidationMessage(externalValidationMessage)}
      </View>
    );
  }
}

const styles = StyleSheet.create({
  loader: {
    right: 15,
    justifyContent: "space-between",
    marginTop: 29
  },
  textInputMain: {
    flexDirection: "column",
    alignContent: "center",
    alignSelf: "center"
  },
  validationMessage: {
    color: "#d46f02"
  },
  seePasswordIcon: {
    color: "#fff",
    paddingTop: 15,
    paddingRight: 5
  },
  seePassword: {
    right: 15,
    justifyContent: "space-between",
    marginTop: 15
  },
  textInputContainer: {
    flexDirection: "row",
    alignSelf: "center"
  },
  content: {
    paddingLeft: 5,
    backgroundColor: "#1c1a1a",
    height: 40,
    borderBottomWidth: 3,
    borderBottomColor: "#d46f02",
    fontFamily: "FranklinGothic",
    color: "white"
  }
});
