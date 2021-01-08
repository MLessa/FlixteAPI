import React, { Component } from "react";
import { View, Animated, StyleSheet } from "react-native";

export default class AnimatedImageBackground extends Component {
  render() {
    const { children, style, imageStyle, imageRef, ...props } = this.props;
    return (
      <View
        accessibilityIgnoresInvertColors={true}
        style={style}
        ref={this._captureRef}
      >
        <Animated.Image
          {...props}
          style={[
            styles.absoluteFill,
            {
              width: style.width,
              height: style.height
            },
            imageStyle
          ]}
          ref={imageRef}
        />
        {children}
      </View>
    );
  }
}

const styles = StyleSheet.create({
   absoluteFill: {
    position: 'absolute',
    left: 0,
    right: 0,
    top: 0,
    bottom: 0,
  }
});
