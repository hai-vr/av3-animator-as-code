using System;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    internal class AacBackingAnimator
    {
        private readonly AacAnimatorGenerator _generator;

        /// <summary> Creates a new backing animator. </summary>
        /// <param name="animatorGenerator"> The generator that will be used to generate the animator. </param>
        public AacBackingAnimator(AacAnimatorGenerator animatorGenerator)
        {
            _generator = animatorGenerator;
        }

        /// <summary> Creates a new boolean parameter in the animator. </summary>
        /// <remarks> If the parameter already exists, it will effectively be returned. </remarks>
        /// <param name="parameterName"> The name of the parameter. </param>
        /// <returns> The created AAC parameter. </returns>
        public AacFlBoolParameter BoolParameter(string parameterName)
        {
            var result = AacFlBoolParameter.Internally(parameterName);
            _generator.CreateParamsAsNeeded(result);
            return result;
        }

        /// <summary> Creates a new trigger parameter in the animator. </summary>
        /// <remarks> If the parameter already exists, it will effectively be returned. </remarks>
        /// <param name="parameterName"> The name of the parameter. </param>
        /// <returns> The created AAC parameter. </returns>
        public AacFlBoolParameter TriggerParameter(string parameterName)
        {
            var result = AacFlBoolParameter.Internally(parameterName);
            _generator.CreateTriggerParamsAsNeeded(result);
            return result;
        }

        /// <summary> Creates a new float parameter in the animator. </summary>
        /// <remarks> If the parameter already exists, it will effectively be returned. </remarks>
        /// <param name="parameterName"> The name of the parameter. </param>
        /// <returns> The created AAC parameter. </returns>
        public AacFlFloatParameter FloatParameter(string parameterName)
        {
            var result = AacFlFloatParameter.Internally(parameterName);
            _generator.CreateParamsAsNeeded(result);
            return result;
        }

        /// <summary> Creates a new integer parameter in the animator. </summary>
        /// <remarks> If the parameter already exists, it will effectively be returned. </remarks>
        /// <param name="parameterName"> The name of the parameter. </param>
        /// <returns> The created AAC parameter. </returns>
        public AacFlIntParameter IntParameter(string parameterName)
        {
            var result = AacFlIntParameter.Internally(parameterName);
            _generator.CreateParamsAsNeeded(result);
            return result;
        }

        /// <summary> Creates a new enum parameter in the animator. </summary>
        /// <remarks> If the parameter already exists, it will effectively be returned. </remarks>
        /// <param name="parameterName"> The name of the parameter. </param>
        /// <typeparam name="TEnum"> The enum type. </typeparam>
        /// <returns> The created AAC parameter. </returns>
        public AacFlEnumIntParameter<TEnum> EnumParameter<TEnum>(string parameterName)
            where TEnum : Enum
        {
            var result = AacFlEnumIntParameter<TEnum>.Internally<TEnum>(parameterName);
            _generator.CreateParamsAsNeeded(result);
            return result;
        }

        /// <summary> Creates a new boolean parameter group in the animator. </summary>
        /// <remarks> If the parameter(s) already exist(s), it will effectively be returned. </remarks>
        /// <param name="parameterNames"> The names of the parameters. </param>
        /// <returns> The created AAC parameter group. </returns>
        public AacFlBoolParameterGroup BoolParameters(params string[] parameterNames)
        {
            var result = AacFlBoolParameterGroup.Internally(parameterNames);
            _generator.CreateParamsAsNeeded(result.ToList().ToArray());
            return result;
        }

        /// <summary> Creates a new trigger parameter group in the animator. </summary>
        /// <remarks> If the parameter(s) already exist(s), it will effectively be returned. </remarks>
        /// <param name="parameterNames"> The names of the parameters. </param>
        /// <returns> The created AAC parameter group. </returns>
        public AacFlBoolParameterGroup TriggerParameters(params string[] parameterNames)
        {
            var result = AacFlBoolParameterGroup.Internally(parameterNames);
            _generator.CreateTriggerParamsAsNeeded(result.ToList().ToArray());
            return result;
        }

        /// <summary> Creates a new float parameter group in the animator. </summary>
        /// <remarks> If the parameter(s) already exist(s), it will effectively be returned. </remarks>
        /// <param name="parameterNames"> The names of the parameters. </param>
        /// <returns> The created AAC parameter group. </returns>
        public AacFlFloatParameterGroup FloatParameters(params string[] parameterNames)
        {
            var result = AacFlFloatParameterGroup.Internally(parameterNames);
            _generator.CreateParamsAsNeeded(result.ToList().ToArray());
            return result;
        }

        /// <summary> Creates a new integer parameter group in the animator. </summary>
        /// <remarks> If the parameter(s) already exist(s), it will effectively be returned. </remarks>
        /// <param name="parameterNames"> The names of the parameters. </param>
        /// <returns> The created AAC parameter group. </returns>
        public AacFlIntParameterGroup IntParameters(params string[] parameterNames)
        {
            var result = AacFlIntParameterGroup.Internally(parameterNames);
            _generator.CreateParamsAsNeeded(result.ToList().ToArray());
            return result;
        }

        /// <inheritdoc cref="BoolParameters(string[])"/>
        /// <param name="parameters"> The list of <see cref="AacFlBoolParameter"/> to use in the list. </param>
        public AacFlBoolParameterGroup BoolParameters(params AacFlBoolParameter[] parameters)
        {
            var result = AacFlBoolParameterGroup.Internally(
                parameters.Select(parameter => parameter.Name).ToArray()
            );
            _generator.CreateParamsAsNeeded(parameters);
            return result;
        }

        /// <inheritdoc cref="TriggerParameters(string[])"/>
        /// <param name="parameters"> The list of <see cref="AacFlBoolParameter"/> to use in the list. </param>
        public AacFlBoolParameterGroup TriggerParameters(params AacFlBoolParameter[] parameters)
        {
            var result = AacFlBoolParameterGroup.Internally(
                parameters.Select(parameter => parameter.Name).ToArray()
            );
            _generator.CreateTriggerParamsAsNeeded(parameters);
            return result;
        }

        /// <inheritdoc cref="FloatParameters(string[])"/>
        /// <param name="parameters"> The list of <see cref="AacFlFloatParameter"/> to use in the list. </param>
        public AacFlFloatParameterGroup FloatParameters(params AacFlFloatParameter[] parameters)
        {
            var result = AacFlFloatParameterGroup.Internally(
                parameters.Select(parameter => parameter.Name).ToArray()
            );
            _generator.CreateParamsAsNeeded(parameters);
            return result;
        }

        /// <inheritdoc cref="IntParameters(string[])"/>
        /// <param name="parameters"> The list of <see cref="AacFlIntParameter"/> to use in the list. </param>
        public AacFlIntParameterGroup IntParameters(params AacFlIntParameter[] parameters)
        {
            var result = AacFlIntParameterGroup.Internally(
                parameters.Select(parameter => parameter.Name).ToArray()
            );
            _generator.CreateParamsAsNeeded(parameters);
            return result;
        }
    }
}
